import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { MatButton } from '@angular/material/button';
import { MatCard } from '@angular/material/card';
import { MatFormField, MatLabel } from '@angular/material/form-field';
import { MatInput } from '@angular/material/input';
import { AccountService } from '../../../core/services/account.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatCard,
    MatFormField,
    MatInput,
    MatLabel,
    MatButton  
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  //best wat to reactform is using formbuilder from angular hence we inject it

  private fb = inject(FormBuilder);
  private accountService = inject(AccountService);
  private router = inject(Router); //redirecting user to another page after login

  //Note: since we r using reactiveform we initialize the logic inside our component.ts

  loginForm = this.fb.group({
    email: [''],
    password: ['']
  })

  onSubmit(){
   this.accountService.login(this.loginForm.value).subscribe({
    next: () => {
      //once we successfully login we going to get user info
      this.accountService.getUserInfo().subscribe();
      this.router.navigateByUrl('/shop');
    }
   })
  }




}
