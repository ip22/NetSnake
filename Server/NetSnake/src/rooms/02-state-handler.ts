import { Room, Client } from "colyseus";
import { Schema, type, MapSchema, ArraySchema } from "@colyseus/schema";

export class Vector2float extends Schema{
    @type("uint32") id = 0;
    @type("uint8") type = 1;
    @type("number") x = Math.floor(Math.random() * 256) - 128;
    @type("number") z = Math.floor(Math.random() * 256) - 128;
}

export class Player extends Schema {
    @type("string") login = "unnamed";
    @type("uint8") skin = 0;
    @type("number") x = Math.floor(Math.random() * 256) - 128;
    @type("number") z = Math.floor(Math.random() * 256) - 128;
    @type("uint8") seg = 0;
    @type("uint16") score = 0;
}

export class State extends Schema {
    @type({ map: Player }) players = new MapSchema<Player>();
    @type([Vector2float]) apples = new ArraySchema<Vector2float>();

    appleLastId=0;   
    gameOverIDs = [];

    createApple(type){
        const apple = new Vector2float();
        apple.id = this.appleLastId++;
        apple.type = type;
        this.apples.push(apple);
    }

    collectApple(player: Player, data: any){
        const apple = this.apples.find((value) => value.id === data.id)
        if(apple === undefined) return;

        apple.x = Math.floor(Math.random() * 256) - 128;
        apple.z = Math.floor(Math.random() * 256) - 128;
        console.log("Apple:" + apple.x + " " + apple.z + "points: " + data.pts);  
        player.score += data.pts;
        if(player.score <= 0) player.score = 0;
        //player.score++;
        console.log(player.score);
        player.seg = player.score;
    }

    createPlayer(sessionId: string, skin: number, login) {   
        const player = new Player();
        player.login = login;
        player.skin = skin;
        this.players.set(sessionId, player);
    }

    removePlayer(sessionId: string) {
        if(this.players.has(sessionId)){          
            this.players.delete(sessionId);
        }
    }

    movePlayer (sessionId: string, data: any) {  
        const player = this.players.get(sessionId);     

        player.x = data.x;
        player.z = data.z;
        //console.log("Player:" + data.x + " " + data.z);  
    }

    gameOver(data) {
        const segementsPositions = JSON.parse(data);
        const clientID = segementsPositions.id;

        const gameOverID = this.gameOverIDs.find((value) => value === clientID)

        if(gameOverID !== undefined) return;

        this.gameOverIDs.push(clientID);
        this.clearGameOverIDs(clientID);
        this.removePlayer(clientID);      

        for(let i = 0; i < segementsPositions.sPs.length; i++){
            const apple = new Vector2float();
            apple.id = this.appleLastId++;
            apple.x = segementsPositions.sPs[i].x
            apple.z = segementsPositions.sPs[i].z
            this.apples.push(apple);
        }
    }

    async clearGameOverIDs(clientID){
        await new Promise(resolve => setTimeout(resolve, 10000));

        const index = this.gameOverIDs.findIndex((v) => v === clientID);

        if(index <= -1) return;

        this.gameOverIDs.splice(index, 1);
    }
}

export class StateHandlerRoom extends Room<State> {
    maxClients = 24;

    startRedCount = 100;
    startGreenCount = 50;
    startBadCount = 50;

    skin = 0;
    skins: number[] = [0];
    
    onCreate (options) {
        this.startRedCount = options.red;
        this.startGreenCount = options.green;
        this.startBadCount = options.bad;

        for(var i = 1; i < options.skins; i++){
            this.skins.push(i);
        }

        this.skin = options.skin;

        this.setState(new State());   
        
        this.onMessage("move",(client, data) => {
            this.state.movePlayer(client.sessionId, data);
        })

        this.onMessage("collect",(client, data) => {
            const player = this.state.players.get(client.sessionId); 
            this.state.collectApple(player, data);
        })

        this.onMessage("gameOver",(client, data)=>{
            console.log("game over");            
            this.state.gameOver(data);
        })

        for(let i = 0; i < this.startRedCount; i++){
            this.state.createApple(1);
        }

        for(let i = 0; i < this.startGreenCount; i++){
            this.state.createApple(2);
        }

        for(let i = 0; i < this.startBadCount; i++){
            this.state.createApple(3);
        }
    }

    //onAuth(client, options, req) {
    //    return true;
    //}

    onJoin (client: Client, data: any) {
        const skin = data.skin;
        this.state.createPlayer(client.sessionId, skin, data.login);
    }

    onLeave (client) {
        this.state.removePlayer(client.sessionId);
    }

    onDispose () {
        console.log("Dispose StateHandlerRoom");
    }
}
