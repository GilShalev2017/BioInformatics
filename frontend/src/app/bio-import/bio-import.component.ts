import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { BioService } from '../services/bio.service';

@Component({
  selector: 'app-bio-import',
  standalone: true,
  imports: [CommonModule, MatButtonModule, MatProgressSpinnerModule],
  templateUrl: './bio-import.component.html',
  styleUrls: ['./bio-import.component.scss']
})
export class BioImportComponent {
  isLoading = false;
  message: string | null = null;
  errorMessage: string | null = null;

  constructor(private bioService: BioService) {}

  triggerImport(): void {
    this.isLoading = true;
    this.message = null;
    this.errorMessage = null;

    this.bioService.importBioData().subscribe({
      next: (response) => {
        this.isLoading = false;
        this.message = 'Bioinformatics data import completed successfully ✅';
      },
      error: (err) => {
        this.isLoading = false;
        this.errorMessage = err.message || 'Import failed. Please try again.';
      }
    });
  }
}
