import { Component, Input, Self } from '@angular/core';
import { ControlValueAccessor, FormControl, NgControl, ReactiveFormsModule } from '@angular/forms';
import { MatError, MatFormField, MatLabel } from '@angular/material/form-field';
import { MatInput } from '@angular/material/input';

@Component({
  selector: 'app-validation-text-re-usable-input',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatFormField,
    MatInput,
    MatError,
    MatLabel
  ],
  templateUrl: './validation-text-re-usable-input.component.html',
  styleUrl: './validation-text-re-usable-input.component.scss'
})

//ControlValueAccessor is use to create a reusable text input 
//and behaviour b/w angular form(reactiveform) and DOM 
export class ValidationTextReUsableInputComponent implements ControlValueAccessor{
  @Input() label = '';
  @Input() type = 'text';
  
   //@Self() this provide us self contained and prevent angular fetching us addition instance
  //NgControl is A base class that binds a FormControl object to a DOM element. 
  constructor(@Self() public ngControl:NgControl ){
    this.ngControl.valueAccessor = this; //we get access to our control when it call upon 
}


  writeValue(obj: any): void {
   
  }
  registerOnChange(fn: any): void {
   
  }
  registerOnTouched(fn: any): void {
    
  }
  
  get control(){
    return this.ngControl.control as FormControl;
  }


}
