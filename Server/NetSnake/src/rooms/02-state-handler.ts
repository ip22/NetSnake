import { Room, Client } from "colyseus";
import { Schema, type, MapSchema } from "@colyseus/schema";

export class Player extends Schema {
    @type("uint8") skin = 0;
    //Math.floor(Math.random() * 256) - 128
    @type("number") x = Math.floor(Math.random() * 256) - 128;
    @type("number") z = Math.floor(Math.random() * 256) - 128;
    @type("uint8") sg = 5;
}

export class State extends Schema {
    @type({ map: Player })
    players = new MapSchema<Player>();

    createApple(){

    }

    createPlayer(sessionId: string, skin: number) {   
        const player = new Player();
        player.skin = skin;
        this.players.set(sessionId, player);
    }

    removePlayer(sessionId: string) {
        this.players.delete(sessionId);
    }

    movePlayer (sessionId: string, data: any) {  
        const player = this.players.get(sessionId);     

        player.x = data.x;
        player.z = data.z;
    }
}

export class StateHandlerRoom extends Room<State> {
    maxClients = 6;
    startAppleCount = 100;
    skin = 0;
    skins: number[] = [0];

    /*mixArray(arr){
        var currentIndex = arr.length;
        var tmpValue, randomIndex;
        
        while(currentIndex !== 0){
            randomIndex = Math.floor(Math.random() * currentIndex);
            currentIndex -= 1;
            tmpValue = arr[currentIndex];
            arr[currentIndex] = arr[randomIndex];
            arr[randomIndex] = tmpValue;
        }
    }*/

    onCreate (options) {
        for(var i = 1; i < options.skins; i++){
            this.skins.push(i);
        }

        this.skin = options.skin;
        //this.mixArray(this.skins);

        this.setState(new State());   
        
        this.onMessage("move",(client, data) =>{
            this.state.movePlayer(client.sessionId, data);
        })
    }

    //onAuth(client, options, req) {
    //    return true;
    //}

    onJoin (client: Client, data: any) {
        const skin = this.skin;
        this.state.createPlayer(client.sessionId, skin);
    }

    onLeave (client) {
        this.state.removePlayer(client.sessionId);
    }

    onDispose () {
        console.log("Dispose StateHandlerRoom");
    }
}
