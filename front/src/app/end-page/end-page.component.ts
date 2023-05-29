import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ApiHelperService } from '../services/api-helper.service';
import { getSessionIdentifier } from '../services/storage.service';

@Component({
  selector: 'app-end-page',
  templateUrl: './end-page.component.html',
  styleUrls: ['./end-page.component.scss']
})
export class EndPageComponent {

  constructor(private api: ApiHelperService, private router: Router) { }

  onHomeClick(): void {
    this.router.navigate(['/home']);
  }

  async downloadRecap() {
    try {
      const response = await this.api.get({ endpoint: '/Session/' + getSessionIdentifier() + '/download', responseType: 'text' });
      const blob = new Blob([response], { type: 'application/csv' });
      const url = window.URL.createObjectURL(blob);
  
      // Nom du fichier
      const fileName = 'recap_'+getSessionIdentifier()+'.csv';
  
      // Création de l'élément <a> avec le lien de téléchargement
      const link = document.createElement('a');
      link.href = url;
      link.download = fileName;
  
      // Ajout de l'élément <a> à la page et déclenchement du téléchargement
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);
    } catch (error) {
      console.log(error);
    }
  }
  
}
