import {Component, OnInit} from '@angular/core';
import {getName} from "../services/storage.service";

@Component({
  selector: 'app-session',
  templateUrl: './session.component.html',
  styleUrls: ['./session.component.scss']
})
export class SessionComponent implements OnInit{

  name: string = "";
  tabCards: string[] = ['1','2','3','5','8','13','∞'];
  chosenCard: string | undefined;
  disabled: boolean = false;

  constructor() {
  }

  ngOnInit(): void {
    this.name = getName();
  }

}
