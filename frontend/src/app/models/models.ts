import { getNgModuleById } from "@angular/core";

export interface Gene {
    Id: number,
    GeneID: string,
    GeneName: string,
    RelatedDiseases: Disease[],
    TargetedByDrugs: Drug[]
}

export interface Disease {
    Id: number,
    DiseaseID: string,
    DiseaseName: string,
    Description: string,
    RelatedGenes: Gene[]
}

export interface Drug {
    Id: number,
    DrugId: string,
    DrugName: string,
    TargetedGenes: Gene[]
}

export interface Relationships {
    Genes: Gene[];
    Diseases: Disease[];
    Drugs: Drug[];
}