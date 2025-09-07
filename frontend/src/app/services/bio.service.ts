
import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, retry } from 'rxjs/operators';
import { Disease, Drug, Gene } from '../models/models';

@Injectable({
  providedIn: 'root'
})
export class BioService {
  private readonly baseBioApi = 'https://localhost:7025/api/Bioinformatics';
  private readonly importApi = 'https://localhost:7025/api/ImportBioData';

  constructor(private http: HttpClient) { }

  getAllGenes(): Observable<Gene[]> {
    return this.http.get<Gene[]>(`${this.baseBioApi}/genes`)
      .pipe(
        // retry(1), // Retry failed request once
        catchError(this.handleError)
      );
  }

  getAllDiseases(): Observable<Disease[]> {
    return this.http.get<Disease[]>(`${this.baseBioApi}/diseases`)
      .pipe(
        catchError(this.handleError)
      );
  }

  getAllDrugs(): Observable<Drug[]> {
    return this.http.get<Drug[]>(`${this.baseBioApi}/drugs`)
      .pipe(
        catchError(this.handleError)
      );
  }

  getDiseaseById(diseaseId: string): Observable<Disease> {
    return this.http.get<Disease>(`${this.baseBioApi}/diseases/${diseaseId}`)
      .pipe(
        catchError(this.handleError)
      );
  }

  getGeneById(geneId: string): Observable<Gene> {
    return this.http.get<Gene>(`${this.baseBioApi}/genes/${geneId}`)
      .pipe(
        catchError(this.handleError)
      );
  }

  getDrugById(drugId: string): Observable<Drug> {
    return this.http.get<Drug>(`${this.baseBioApi}/drugs/${drugId}`)
      .pipe(
        catchError(this.handleError)
      );
  }

  searchDiseases(query: string): Observable<Disease[]> {
    return this.http.get<Disease[]>(`${this.baseBioApi}/search/diseases?query=${encodeURIComponent(query)}`)
      .pipe(
        catchError(this.handleError)
      );
  }

  searchGenes(query: string): Observable<Gene[]> {
    return this.http.get<Gene[]>(`${this.baseBioApi}/search/genes?query=${encodeURIComponent(query)}`)
      .pipe(
        catchError(this.handleError)
      );
  }

  searchDrugs(query: string): Observable<Drug[]> {
    return this.http.get<Drug[]>(`${this.baseBioApi}/search/drugs?query=${encodeURIComponent(query)}`)
      .pipe(
        catchError(this.handleError)
      );
  }

  importBioData(): Observable<any> {
    return this.http.post(`https://localhost:7025/api/ImportBioData/import`,{})
      .pipe(
        catchError(this.handleError)
      );
  }

  elasticSearchDiseases(query: string): Observable<Disease[]> {
    return this.http.get<Disease[]>(`${this.baseBioApi}/elasticsearch/diseases?query=${encodeURIComponent(query)}`)
      .pipe(
        catchError(this.handleError)
      );
  }

  // Private method to handle HTTP errors
  private handleError(error: HttpErrorResponse): Observable<never> {
    let errorMessage = 'An unknown error occurred';

    if (error.error instanceof ErrorEvent) {
      // Client-side error
      errorMessage = `Client Error: ${error.error.message}`;
    } else {
      // Server-side error
      switch (error.status) {
        case 0:
          errorMessage = 'Unable to connect to the server. Please check your internet connection.';
          break;
        case 400:
          errorMessage = 'Bad Request: The server could not understand the request.';
          break;
        case 401:
          errorMessage = 'Unauthorized: Please log in to access this resource.';
          break;
        case 403:
          errorMessage = 'Forbidden: You do not have permission to access this resource.';
          break;
        case 404:
          errorMessage = 'Not Found: The requested resource was not found.';
          break;
        case 500:
          errorMessage = 'Internal Server Error: Something went wrong on the server.';
          break;
        case 503:
          errorMessage = 'Service Unavailable: The server is temporarily unavailable.';
          break;
        default:
          errorMessage = `Server Error: ${error.status} - ${error.message}`;
      }
    }

    console.error('HTTP Error:', error);
    return throwError(() => new Error(errorMessage));
  }
}