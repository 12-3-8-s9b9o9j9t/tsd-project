import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-board',
  templateUrl: './board.component.html',
  styleUrls: ['./board.component.scss']
})
export class BoardComponent {

  players: string[] = ['Yanis','Hector',"Jack","Georges"];
  player_cards: string[] | undefined;  // To be filled by the server

  constructor() { }

}
