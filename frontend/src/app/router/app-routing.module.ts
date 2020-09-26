import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HeroesComponent } from '../components/heroes/heroes.component';
import { PageNotFoundComponent } from '../components/page-not-found/page-not-found.component';
import { HomeComponent } from '../components/home/home.component';
import { PowersComponent } from '../components/powers/powers.component';
import { MainComponent } from '../components/main/main.component';
import { LoginComponent } from '../components/login/login.component';
import { RegisterComponent } from '../components/register/register.component';
import { AuthGuard } from '../helpers/auth.guard'
import { MyProfileComponent } from '../components/my-profile/my-profile.component';
import { ResetPasswordComponent } from '../components/reset-password/reset-password.component';
import { UnauthorizedComponent } from '../components/unauthorized/unauthorized.component';

const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'resetPassword', component: ResetPasswordComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'unauthorized', component: UnauthorizedComponent },
  {
    path: 'main', component: MainComponent, canActivate: [AuthGuard],
    children: [
      {
        path: 'heroes',
        component: HeroesComponent
      },
      {
        path: 'powers',
        component: PowersComponent,
      },
      {
        path: 'home',
        component: HomeComponent,
      },
      {
        path: 'myProfile',
        component: MyProfileComponent
      }
    ]
  },
  { path: '**', component: PageNotFoundComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }