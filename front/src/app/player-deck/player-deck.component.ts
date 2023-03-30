import { Component } from '@angular/core';
import { ApiHelperService } from '../services/api-helper.service';

@Component({
  selector: 'app-player-deck',
  templateUrl: './player-deck.component.html',
  styleUrls: ['./player-deck.component.scss']
})
export class PlayerDeckComponent {

  name: string = "";
  tabCards: string[] = ['1', '2', '3', '5', '8', '13', '∞'];
  selectedCard: string | undefined;
  disabled: boolean = false;

  constructor(private api: ApiHelperService) { }

  validate(): void {
    this.disabled = true;

    this.api.post({
      endpoint: 'session',
      data: {
        name: this.name,
        card: this.selectedCard
      }
    }).then((response) => {
      console.log(response);
    }).catch((error) => {
      console.log(error);
    });
  }

}
