import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-end-page',
  templateUrl: './end-page.component.html',
  styleUrls: ['./end-page.component.scss']
})
export class EndPageComponent {

  constructor(private router: Router) { }

  onHomeClick(): void {
    this.router.navigate(['/home']);
  }
}
