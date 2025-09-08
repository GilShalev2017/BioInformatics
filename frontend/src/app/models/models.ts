import { getNgModuleById } from "@angular/core";

export interface Gene {
    Id: number;
    GeneID: string;
    GeneName: string;
    RelatedDiseases?: Disease[];
    TargetedByDrugs?: Drug[];

    DrugGenes?: DrugGene[];
    DiseaseGenes?: DiseaseGene[];
}

export interface Disease {
    Id: number;
    DiseaseID: string;
    DiseaseName: string;
    Description: string;
    RelatedGenes?: Gene[];

    DiseaseGenes?: DiseaseGene[];
}

export interface Drug {
    Id: number;
    DrugID: string;

    DrugName: string;
    TargetedGenes?: Gene[];

    DrugGenes?: DrugGene[];
}

export interface Relationships {
    Genes: Gene[];
    Diseases: Disease[];
    Drugs: Drug[];
}

export interface DiseaseGene {
    DiseaseID: string;
    Disease: Disease;
    GeneID: string;
    Gene: Gene;
    EvidenceType: string;
    Strength: number;
}

export interface DrugGene
{
    DrugID :string;
    Drug: Drug;
    GeneID: string;
    Gene: Gene;
    Effect: string;
    ApprovalYear: string;
}