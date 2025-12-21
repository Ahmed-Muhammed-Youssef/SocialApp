import { Component, inject, signal, computed } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators, AbstractControl, FormControl } from '@angular/forms';
import { AuthService } from '../services/auth';
import { Router, RouterModule } from '@angular/router';
import { LoaderService } from '../../shared/services/loader';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatSelectModule } from '@angular/material/select';
import { provideNativeDateAdapter } from '@angular/material/core';
import { DataReferenceService } from '../../shared/services/data-reference.service';
import { CityDTO } from '../../shared/models/city-dto';

@Component({
  selector: 'app-signup',
  standalone: true,
  providers: [provideNativeDateAdapter()],
  imports: [
    ReactiveFormsModule, RouterModule, MatFormFieldModule, MatInputModule, 
    MatIconModule, MatButtonModule, MatAutocompleteModule, MatDatepickerModule, MatSelectModule
  ],
  templateUrl: './signup.html',
  styleUrl: './signup.css',
})
export class Signup {
  private fb = inject(FormBuilder);
  private auth = inject(AuthService);
  private router = inject(Router);
  private loader = inject(LoaderService);
  private cityService = inject(DataReferenceService);

  errorMessage = signal<string | null>(null);
  hidePassword = signal(true);
  hideConfirm = signal(true);
  citySearchQuery = signal('');

  genderOptions = [
    { value: 1, label: 'Male' },
    { value: 2, label: 'Female' },
    { value: 0, label: 'Other' }
  ];

  private allCities = signal(<CityDTO[]>[]);

  constructor() {
    this.cityService.getCities().subscribe({
      next: (cities) => {
        this.allCities.set(cities);
      }
    });
  }

  filteredCities = computed(() => {
    const query = this.citySearchQuery().toLowerCase();
    return this.allCities().filter(c => c.name.toLowerCase().includes(query));
  });

  registerForm = this.fb.group({
    firstName: ['', [Validators.required]],
    lastName: ['', [Validators.required]],
    email: ['', [Validators.required, Validators.email]],
    gender: [null as number | null, [Validators.required]],
    dateOfBirth: ['', [Validators.required]],
    cityId: [null as number | null, [Validators.required]], 
    cityName: ['', [Validators.required]], 
    password: ['', [Validators.required, Validators.minLength(6)]],
    confirmPassword: ['', [Validators.required]]
  }, { 
    validators: (group: AbstractControl) => {
      const pass = group.get('password')?.value;
      const confirm = group.get('confirmPassword')?.value;
      return pass === confirm ? null : { mismatch: true };
    }
  });

  onCitySelected(city: {id: number, name: string}) {
    this.registerForm.patchValue({ 
      cityId: city.id,
      cityName: city.name 
    });
  }

  onSubmit() {
    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();
      return;
    }

    this.loader.show();
    
    // We use getRawValue() to avoid "null" type errors with strict forms
    const rawValues = this.registerForm.getRawValue();
    
    const payload = {
      firstName: rawValues.firstName!,
      lastName: rawValues.lastName!,
      email: rawValues.email!,
      gender: rawValues.gender!,
      cityId: rawValues.cityId!,
      password: rawValues.password!,
      dateOfBirth: new Date(rawValues.dateOfBirth!).toISOString()
    };

    this.auth.registerUser(payload).subscribe({
      next: () => {
        this.loader.hide();
        this.router.navigate(['/newsfeed']);
      },
      error: () => {
        this.loader.hide();
        this.errorMessage.set('Registration failed. Please check your data.');
      }
    });
  }
}