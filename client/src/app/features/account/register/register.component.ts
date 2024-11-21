import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButton } from '@angular/material/button';
import { MatCard } from '@angular/material/card';
import { MatError, MatFormField, MatLabel } from '@angular/material/form-field';
import { MatInput } from '@angular/material/input';
import { AccountService } from '../../../core/services/account.service';
import { Router } from '@angular/router';
import { SnarkbarService } from '../../../core/services/snarkbar.service';
import { JsonPipe } from '@angular/common';
import { ValidationTextReUsableInputComponent } from "../../../shared/components/validation-text-re-usable-input/validation-text-re-usable-input.component";

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatCard,
    MatButton, 
    ValidationTextReUsableInputComponent
   
],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent {
  
  private fb = inject(FormBuilder);
  private accountService = inject(AccountService);
  private router = inject(Router);
  private snackService = inject(SnarkbarService);
  validationErrors?: string[]



  registerForm = this.fb.group({
    firstName: ['', Validators.required],
    lastName: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [
      Validators.required, 
      Validators.pattern('(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*?&].{4,8}')]]
  })

  onSubmit(){
   this.accountService.register(this.registerForm.value).subscribe({
    next: () => {
      //once we successfully
      this.snackService.success('Registeration successful - you can now login')
      this.router.navigateByUrl('/account/login');
    },
    error: errors => this.validationErrors = errors
   })
  }
}
