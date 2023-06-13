import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { SessionComponent } from './session/session.component';
import { WaitingRoomComponent } from './waiting-room/waiting-room.component';
import { EndPageComponent } from './end-page/end-page.component';
import { LoginComponent } from './login/login.component';
import { HistoryComponent } from './history/history.component';
import { TermsAndCondsComponent } from './terms-and-conds/terms-and-conds.component';

const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component : LoginComponent },
  { path: 'home', component: HomeComponent },
  { path: 'history', component: HistoryComponent },
  { path: 'session/:id/waiting-room', component: WaitingRoomComponent },
  { path: 'session/:id/game', component: SessionComponent },
  { path: 'session/:id/end', component: EndPageComponent },
  { path: 'legal/terms', component: TermsAndCondsComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
