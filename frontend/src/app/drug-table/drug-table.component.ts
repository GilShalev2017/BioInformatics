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

import { Drug } from '../models/models';
import { BioService } from '../services/bio.service';

@Component({
  selector: 'app-drug-table',
  templateUrl: './drug-table.component.html',
  styleUrls: ['./drug-table.component.scss'],
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
export class DrugTableComponent implements OnInit {
  displayedColumns: string[] = ['DrugId', 'DrugName'];
  dataSource = new MatTableDataSource<Drug>([]);

  isLoading = false;
  errorMessage: string | null = null;
  searchTerm = '';
  selectedDrug: Drug | null = null;

  @ViewChild(MatPaginator, { static: false }) set matPaginator(paginator: MatPaginator) {
    if (paginator) this.dataSource.paginator = paginator;
  }

  @ViewChild(MatSort, { static: false }) set matSort(sort: MatSort) {
    if (sort) this.dataSource.sort = sort;
  }

  constructor(private bioService: BioService) { }

  ngOnInit(): void {
    this.loadDrugs();

    this.dataSource.filterPredicate = (data: Drug, filter: string) =>
      data.DrugId.toLowerCase().includes(filter) ||
      data.DrugName.toLowerCase().includes(filter);
  }

  loadDrugs(): void {
    this.isLoading = true;
    this.errorMessage = null;

    this.bioService.getAllDrugs().subscribe({
      next: drugs => {
        this.dataSource.data = drugs;
        this.isLoading = false;
      },
      error: () => {
        this.errorMessage = 'Failed to load drugs.';
        this.isLoading = false;
      }
    });
  }

  onSearchChange(event: Event): void {
    const input = event.target as HTMLInputElement;
    this.searchTerm = input.value.trim().toLowerCase();
    this.dataSource.filter = this.searchTerm;
  }

  onSelectDrug(drug: Drug): void {
    this.selectedDrug = drug;
  }

  clearSelection(): void {
    this.selectedDrug = null;
  }
}
