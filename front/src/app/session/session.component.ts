import {Component, OnInit} from '@angular/core';
import {getName} from "../services/storage.service";

@Component({
  selector: 'app-session',
  templateUrl: './session.component.html',
  styleUrls: ['./session.component.scss']
})
export class SessionComponent implements OnInit{

  name: string = "";

  constructor() {
  }

  ngOnInit(): void {
    this.name = getName();
  }



}
