import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment'; // Ajuste o caminho conforme sua pasta

@Injectable({
    providedIn: 'root'
})
export class TurmaService {
    private apiUrl = `${environment.gatewayUrl}/api/Turma`;

    constructor(private http: HttpClient) { }

    getTurmas() {
        return this.http.get(this.apiUrl);
    }
}