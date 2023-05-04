import { Injectable, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../environments/environment';
import { getSessionIdentifier } from './storage.service';
// import { WebSocket as ws } from 'ws';

@Injectable({
	providedIn: 'root'
})
export class SocketService {

	base_ws_url = environment.ws_url;
	private socket!: WebSocket;

	public connect(): void {
		const sessionIdentifier: string = getSessionIdentifier();
		this.socket = new WebSocket(this.base_ws_url + "/" + sessionIdentifier);

		this.socket.onopen = () => {
			console.log("Websocket connected");
		}
	}

	public disconnect(): void {
		this.socket.close();
		console.log("Websocket disconnected");
	}

	public onMessage(): Observable<any> {
		return new Observable<any>(observer => {
			this.socket.onmessage = (event) => {
				console.log("Message received");
      			console.log(JSON.parse(event.data));
				observer.next(JSON.parse(event.data));
			}
		});
	}

}
