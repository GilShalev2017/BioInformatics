import { Component, OnDestroy, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BioDataServiceService } from '../services/bio-data.service.service';
import { GeneViewModel, DbLink } from '../models/models';
import {
  Subject,
  debounceTime,
  distinctUntilChanged,
  filter,
  switchMap,
  takeUntil,
} from 'rxjs';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-single-gene-page',
  standalone: true,
  templateUrl: './single-gene-page.component.component.html',
  styleUrls: ['./single-gene-page.component.component.scss'],
  imports: [
    CommonModule,
    FormsModule,
    MatButtonModule,
    MatProgressSpinnerModule,
  ],
})
export class SingleGenePageComponentComponent implements OnInit, OnDestroy {
  constructor(private bioDataService: BioDataServiceService) {}

  searchText = '';
  selectedGene?: GeneViewModel;
  searchResults: GeneViewModel[] = [];

  private search$ = new Subject<string>();
  private destroy$ = new Subject<void>();
  isLoading = false;

  ngOnInit(): void {
    // Optional initial load
    this.loadGene('A2M');

    this.search$
      .pipe(
        debounceTime(300),
        distinctUntilChanged(),
        filter((q) => q.length >= 2),
        switchMap((q) => this.bioDataService.searchGenes(q)),
        takeUntil(this.destroy$)
      )
      .subscribe((genes) => {
        this.searchResults = genes;
      });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  onSearchChange(value: string) {
    this.search$.next(value.trim());
  }

  selectGene(symbol: string) {
    // this.searchResults = [];
    this.searchText = symbol;
    this.loadGene(symbol);
  }

  private loadGene(symbol: string) {
    this.bioDataService
      .loadGeneDetails(symbol)
      .subscribe((gene) => (this.selectedGene = gene));
  }

  buildUrl(link: DbLink): string {
    switch (link.SourceDb.toLowerCase()) {
      case 'entrez':
        return `https://www.ncbi.nlm.nih.gov/gene/${link.SourceDbId}`;
      case 'ensembl':
        return `https://www.ensembl.org/id/${link.SourceDbId}`;
      case 'uniprot':
        return `https://www.uniprot.org/uniprot/${link.SourceDbId}`;
      default:
        return '#';
    }
  }

  loadData() {
    this.isLoading = true;
    this.bioDataService.loadData().subscribe({
      next: () => {
        this.isLoading = false;
      },
      error: () => {
        this.isLoading = false;
      },
    });
  }
}
