import { Room, Client } from "colyseus";
import { Schema, type, MapSchema } from "@colyseus/schema";

export class Player extends Schema {
    //Math.floor(Math.random() * 256) - 128
    @type("number") x = 0;
    @type("number") z = 0;
    @type("uint8") sg = 5;
}

export class State extends Schema {
    @type({ map: Player })
    players = new MapSchema<Player>();

    something = "This attribute won't be sent to the client-side";

    createPlayer(sessionId: string) {   
        const player = new Player();
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

    onCreate (options) {
        console.log("StateHandlerRoom created!");
        
        this.setState(new State());   
        
        this.onMessage("move",(client, dada) =>{
            this.state.movePlayer(client.sessionId, dada);
        })
    }

    //onAuth(client, options, req) {
    //    return true;
    //}

    onJoin (client: Client, data: any) {
        this.state.createPlayer(client.sessionId);
    }

    onLeave (client) {
        this.state.removePlayer(client.sessionId);
    }

    onDispose () {
        console.log("Dispose StateHandlerRoom");
    }
}
