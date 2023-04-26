import { Injectable, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../environments/environment';
// import { WebSocket as ws } from 'ws';

@Injectable({
	providedIn: 'root'
})
export class SocketService implements OnInit {

	api_url = environment.ws_url;
	private socket!: WebSocket;


	constructor() {
		this.connect();
	}

	ngOnInit(): void {
		console.log("websocket initialized")
		this.onMessage().subscribe(message => {
			console.log('Received message:', message);
		});
	}

	public connect(): void {
		this.socket = new WebSocket(this.api_url);
	}

	public disconnect(): void {
		this.socket.close();
	}

	public onMessage(): Observable<any> {
		return new Observable<any>(observer => {
			this.socket.onmessage = (event) => observer.next(event.data);
		});
	}

}
