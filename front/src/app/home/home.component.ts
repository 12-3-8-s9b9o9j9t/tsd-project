import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent {

  nameControl = new FormControl('', [Validators.required]);

  formGroup = new FormGroup({
    name: this.nameControl,
  });

  constructor(   
    private router: Router
  ) { }


  enter(): void {
    this.router.navigateByUrl('/session');
  }


}
