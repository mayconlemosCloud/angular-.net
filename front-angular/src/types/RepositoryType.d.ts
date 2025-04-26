export interface RepositoryType {
  list(): Observable<any[]>;
  get(id: any): Observable<any>;
  create(data: any): Observable<any>;
  update(id: any, data: any): Observable<any>;
  delete(id: any): Observable<any>;
}
