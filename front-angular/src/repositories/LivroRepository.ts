import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../environments/environment';



@Injectable({
  providedIn: 'root',
})
export class LivroRepository {
  private rota = `${environment.apiUrl}/livro`;
  constructor(private http: HttpClient) {}

  list(): Observable<any[]> {
    return this.http.get<any[]>(this.rota);
  }

  get(id: any): Observable<any> {
    return this.http.get<any>(`${this.rota}/${id}`);
  }

  create(data: any): Observable<any> {
    return this.http.post<any>(this.rota, data);
  }

  update(id: any, data: any): Observable<any> {
    return this.http.put<any>(`${this.rota}/${id}`, data);
  }

  delete(id: any): Observable<any> {
    return this.http.delete<any>(`${this.rota}/${id}`);
  }
}
