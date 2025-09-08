import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

import { Disease } from '../models/models';
import { BioService } from '../services/bio.service';
import { MatIcon } from '@angular/material/icon';

import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-disease-table',
  templateUrl: './disease-table.component.html',
  styleUrls: ['./disease-table.component.scss'],
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
    MatProgressSpinnerModule,
    // MatIcon,
    [FormsModule]
  ]
})
export class DiseaseTableComponent implements OnInit, AfterViewInit {
  displayedColumns: string[] = ['DiseaseID', 'DiseaseName', 'Description'];
  dataSource = new MatTableDataSource<Disease>([]);

  isLoading = false;
  errorMessage: string | null = null;
  searchTerm = '';
  selectedDisease: Disease | null = null;

  @ViewChild(MatPaginator, { static: false }) set matPaginator(paginator: MatPaginator) {
    if (paginator) this.dataSource.paginator = paginator;
  }

  @ViewChild(MatSort, { static: false }) set matSort(sort: MatSort) {
    if (sort) this.dataSource.sort = sort;
  }

  totalCount = 0;
  pageSize = 10;

  constructor(private bioService: BioService) { }

  ngOnInit(): void {
    this.loadDiseases();
    //this.loadPagedDiseases();
  }

  ngAfterViewInit(): void {
    // Define filter predicate once
    this.dataSource.filterPredicate = (data: Disease, filter: string) =>
      data.DiseaseID.toLowerCase().includes(filter) ||
      data.DiseaseName.toLowerCase().includes(filter) ||
      (data.Description?.toLowerCase().includes(filter) ?? false);
  }

  loadDiseases(): void {
    this.isLoading = true;
    this.errorMessage = null;

    this.bioService.getAllDiseases().subscribe({
      next: (diseases) => {
        console.log('Diseases from server:', diseases);  // <-- debug
        this.dataSource.data = diseases;
        this.isLoading = false;
      },
      error: () => {
        this.errorMessage = 'Failed to load diseases.';
        this.isLoading = false;
      }
    });
  }

  onSearchChange(event: Event): void {
    const input = event.target as HTMLInputElement;
    this.searchTerm = input.value.trim().toLowerCase();
    this.dataSource.filter = this.searchTerm;
  }

  onSelectDisease(disease: Disease): void {
    this.selectedDisease = disease;
    console.log( `SelectedDisease: ${this.selectedDisease}`);
  }

  clearSelection(): void {
    this.selectedDisease = null;
  }

  elasticSearch() {
    if (!this.searchTerm)
      return;

    this.bioService.elasticSearchDiseases(this.searchTerm)
      .subscribe(result => this.dataSource.data = result);
  }

  // loadPagedDiseases() {
  //   this.bioService.getDiseasesPaged(this.dataSource.paginator?.pageIndex ?? 0, this.pageSize, this.searchTerm)
  //     .subscribe(pagedDiseases => {
  //       this.dataSource.data = pagedDiseases.Items;
  //       this.totalCount = pagedDiseases.TotalCount;
  //     });
  // }

  // onPagedSearchChange(event: Event) {
  //   const input = event.target as HTMLInputElement;
  //   this.searchTerm = input.value.trim().toLowerCase();
  //   //this.dataSource.paginator?.pageIndex = 0; // reset to first page
  //   this.loadPagedDiseases();
  // }

  // onPageChange() {
  //   this.loadPagedDiseases();
  // }
}
