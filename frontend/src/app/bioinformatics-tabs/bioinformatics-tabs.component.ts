import { DiseaseTableComponent } from '../disease-table/disease-table.component';
import { Component, OnInit } from '@angular/core';
import { MatTabsModule } from '@angular/material/tabs';
import { BioImportComponent } from '../bio-import/bio-import.component';
import { GeneTableComponent } from "../gene-table/gene-table.component";
import { DrugTableComponent } from '../drug-table/drug-table.component';
import { RelationshipsGraphComponent } from '../relationships/relationships.component';

@Component({
  selector: 'app-bioinformatics-tabs',
  templateUrl: './bioinformatics-tabs.component.html',
  styleUrls: ['./bioinformatics-tabs.component.scss'],
  standalone: true,
  imports: [
    MatTabsModule,
    DiseaseTableComponent,
    BioImportComponent,
    GeneTableComponent,
    DrugTableComponent,
    RelationshipsGraphComponent
]
})
export class BioinformaticsTabsComponent implements OnInit {
  
  activeTab: 'genes' | 'diseases' | 'drugs' | 'relationships' = 'genes';

  lastUpdated = new Date();

  tabs = [
    { id: 'genes', label: 'Genes', icon: '🧬' },
    { id: 'diseases', label: 'Diseases', icon: '🦠' },
    { id: 'drugs', label: 'Drugs', icon: '💊' },
    { id: 'relationships', label: 'Relationships', icon: '🔗' }
  ];

  ngOnInit(): void {
  }

  setActiveTab(tabId: 'genes' | 'diseases' | 'drugs' | 'relationships'): void {
    this.activeTab = tabId;
  }
 
}