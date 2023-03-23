import {Component, OnInit} from '@angular/core';
import {getName} from "../services/storage.service";
import { HttpClient } from '@angular/common/http';

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

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.name = getName();
  }

  validate(): void {
    this.disabled = true;
    this.http.post('http://localhost:3000/session', {name: this.name, card: this.chosenCard}).subscribe(
      (response) => {
        console.log(response);
      }
    );
  }

}
