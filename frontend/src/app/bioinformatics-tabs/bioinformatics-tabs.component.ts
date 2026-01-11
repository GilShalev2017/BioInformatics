import { DiseaseTableComponent } from '../disease-table/disease-table.component';
import { Component, OnInit } from '@angular/core';
import { MatTabsModule } from '@angular/material/tabs';
import { BioImportComponent } from '../bio-import/bio-import.component';
import { GeneTableComponent } from "../gene-table/gene-table.component";
import { DrugTableComponent } from '../drug-table/drug-table.component';
import { RelationshipsGraphComponent } from '../relationships/relationships.component';
import { SingleGenePageComponentComponent } from '../single-gene-page/single-gene-page.component.component';

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
    RelationshipsGraphComponent,
    SingleGenePageComponentComponent
]
})
export class BioinformaticsTabsComponent implements OnInit {
  
  activeTab: 'genes' | 'diseases' | 'drugs' | 'relationships' | 'single-gene'= 'genes';

  lastUpdated = new Date();

  tabs = [
    { id: 'genes', label: 'Genes', icon: '🧬' },
    { id: 'diseases', label: 'Diseases', icon: '🦠' },
    { id: 'drugs', label: 'Drugs', icon: '💊' },
    { id: 'relationships', label: 'Relationships', icon: '🔗' },
    { id: 'single-gene', label: 'Single Gene', icon: '🧬' }
  ];

  ngOnInit(): void {
  }

  setActiveTab(tabId: 'genes' | 'diseases' | 'drugs' | 'relationships' | 'single-gene'): void {
    this.activeTab = tabId;
  }
 
}