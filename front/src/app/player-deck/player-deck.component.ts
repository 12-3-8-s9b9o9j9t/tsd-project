import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';

@Component({
  selector: 'app-player-deck',
  templateUrl: './player-deck.component.html',
  styleUrls: ['./player-deck.component.scss']
})
export class PlayerDeckComponent {

  name: string = "";
  tabCards: string[] = ['1','2','3','5','8','13','∞'];
  selectedCard: string | undefined;
  disabled: boolean = false;

  constructor(private http: HttpClient) { }

  validate(): void {
    this.disabled = true;
    this.http.post('http://localhost:3000/session', {name: this.name, card: this.selectedCard}).subscribe(
      (response) => {
        console.log(response);
      }
    );
  }

}
