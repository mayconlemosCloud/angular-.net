import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LivroService {
  private baseUrl = '/api/livros';

  constructor(private http: HttpClient) {}

  list(): Observable<any[]> {
    return this.http.get<any[]>(this.baseUrl);
  }

  create(livro: any): Observable<any> {
    return this.http.post(this.baseUrl, livro);
  }

  update(id: number, livro: any): Observable<any> {
    return this.http.put(`${this.baseUrl}/${id}`, livro);
  }

  delete(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/${id}`);
  }
}
