import { Injectable } from '@angular/core';
import { environment } from '../../environment/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { GeneViewModel } from '../models/models';

@Injectable({
  providedIn: 'root'
})
export class BioDataServiceService {
  
  private readonly flyMapsBaseUrl = environment.flyMapsApiBaseUrl;
  constructor(private http: HttpClient  ) { }

  loadGeneDetails(geneId: string): Observable<GeneViewModel> {
    return this.http.get<any>(`${this.flyMapsBaseUrl}/BioData/genes/${geneId}`);
  }

  searchGenes(query: string): Observable<GeneViewModel[]> {
    return this.http.get<GeneViewModel[]>(`${this.flyMapsBaseUrl}/BioData/genes/search?query=${query}`);
  } 
  
  loadData(): Observable<any> {
    return this.http.post<any>(`${this.flyMapsBaseUrl}/BioData/import`,{});
  }
}
