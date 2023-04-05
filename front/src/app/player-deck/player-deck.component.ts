import { Component, OnInit } from '@angular/core';
import { ApiHelperService } from '../services/api-helper.service';
import { getID } from '../services/storage.service';

@Component({
  selector: 'app-player-deck',
  templateUrl: './player-deck.component.html',
  styleUrls: ['./player-deck.component.scss']
})
export class PlayerDeckComponent implements OnInit {

  name: string = "";
  tabCards: string[] = ['1', '2', '3', '5', '8', '13', '∞'];
  selectedCard: string | undefined;
  disabled: boolean = false;
  hasVoted: boolean = false;

  constructor(private api: ApiHelperService) { }

  ngOnInit(): void {
    this.refreshPlayerDeck();
  }

  validate(): void {
    this.disabled = true;

    this.api.post({
      endpoint: '/Session/voteCurrentUserStory/'+getID()+'/'+this.selectedCard
    }).then((response) => {
      console.log("Vote sent");
      console.log(response);
      this.hasVoted = true;
    }).catch((error) => {
      console.log("Vote not sent");
      console.log(error);
    });
  }

  refreshPlayerDeck(): void {
    this.api.get({
      endpoint: '/Session'
    }).then((response) => {
      if (response.state == "voting" && !this.hasVoted) {
        this.disabled = false;
      }
      if (response.state == "discussing") {
        this.disabled = true;
        this.hasVoted = false;
      }
    }).catch((error) => {
      console.log(error);
    });

    setTimeout(() => {this.refreshPlayerDeck();} , 1000);
  }
}
