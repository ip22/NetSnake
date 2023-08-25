import { Room, Client } from "colyseus";
import { Schema, type, MapSchema } from "@colyseus/schema";

export class Player extends Schema {

}

export class State extends Schema {
    @type({ map: Player })
    players = new MapSchema<Player>();

    something = "This attribute won't be sent to the client-side";

    createPlayer(sessionId: string, data: any, skin: number) {
        const player = new Player();       

        this.players.set(sessionId, player);
    }

    removePlayer(sessionId: string) {
        this.players.delete(sessionId);
    }

    movePlayer (sessionId: string, data: any) {  
        const player = this.players.get(sessionId);      

    }
}

export class StateHandlerRoom extends Room<State> {
    maxClients = 2;

    skins: number[] = [0];

    mixArray(arr){
        var currentIndex = arr.length;
        var tmpValue, randomIndex;
        
        while(currentIndex !== 0){
            randomIndex = Math.floor(Math.random() * currentIndex);
            currentIndex -= 1;
            tmpValue = arr[currentIndex];
            arr[currentIndex] = arr[randomIndex];
            arr[randomIndex] = tmpValue;
        }
    }

    onCreate (options) {
        for(var i = 1; i < options.skins; i++){
            this.skins.push(i);
        }

        this.mixArray(this.skins);
    
    }

    onAuth(client, options, req) {
        return true;
    }

    onJoin (client: Client, data: any) {
        if(this.clients.length > 1) this.lock();

        const skin = this.skins[this.clients.length - 1];
        this.state.createPlayer(client.sessionId, data, skin);
    }

    onLeave (client) {
        this.state.removePlayer(client.sessionId);
    }

    onDispose () {
        console.log("Dispose StateHandlerRoom");
    }

}
