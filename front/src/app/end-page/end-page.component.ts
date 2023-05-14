import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ApiHelperService } from '../services/api-helper.service';

@Component({
  selector: 'app-end-page',
  templateUrl: './end-page.component.html',
  styleUrls: ['./end-page.component.scss']
})
export class EndPageComponent {

  constructor(private api: ApiHelperService, private router: Router) { }

  onHomeClick(): void {

    //// To be deleted
    this.api.post({ endpoint: '/Session/createSession' }).then((response) => { }).catch((error) => { console.log(error); });

    this.router.navigate(['/home']);
  }
}
