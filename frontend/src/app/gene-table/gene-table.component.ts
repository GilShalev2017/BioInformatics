import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

import { Gene } from '../models/models';
import { BioService } from '../services/bio.service';

@Component({
  selector: 'app-gene-table',
  templateUrl: './gene-table.component.html',
  styleUrls: ['./gene-table.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatCardModule,
    MatProgressSpinnerModule
  ]
})
export class GeneTableComponent implements OnInit {
  displayedColumns: string[] = ['GeneID', 'GeneName'];
  dataSource = new MatTableDataSource<Gene>([]);

  isLoading = false;
  errorMessage: string | null = null;
  searchTerm = '';
  selectedGene: Gene | null = null;

  @ViewChild(MatPaginator, { static: false }) set matPaginator(paginator: MatPaginator) {
    if (paginator) this.dataSource.paginator = paginator;
  }

  @ViewChild(MatSort, { static: false }) set matSort(sort: MatSort) {
    if (sort) this.dataSource.sort = sort;
  }

  constructor(private bioService: BioService) { }

  ngOnInit(): void {
    this.loadGenes();

    this.dataSource.filterPredicate = (data: Gene, filter: string) =>
      data.GeneID.toLowerCase().includes(filter) ||
      data.GeneName.toLowerCase().includes(filter);
  }

  loadGenes(): void {
    this.isLoading = true;
    this.errorMessage = null;

    this.bioService.getAllGenes().subscribe({
      next: genes => {
        this.dataSource.data = genes;
        this.isLoading = false;
      },
      error: () => {
        this.errorMessage = 'Failed to load genes.';
        this.isLoading = false;
      }
    });
  }

  onSearchChange(event: Event): void {
    const input = event.target as HTMLInputElement;
    this.searchTerm = input.value.trim().toLowerCase();
    this.dataSource.filter = this.searchTerm;
  }

  onSelectGene(gene: Gene): void {
    this.selectedGene = gene;
  }

  clearSelection(): void {
    this.selectedGene = null;
  }
}
