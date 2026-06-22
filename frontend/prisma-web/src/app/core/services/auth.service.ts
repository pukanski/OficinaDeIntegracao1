import { Injectable } from '@angular/core';
import { createClient, SupabaseClient, User } from '@supabase/supabase-js';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { firstValueFrom } from 'rxjs';

export type UserRole = 'professor' | 'aluno' | 'admin';

@Injectable({
    providedIn: 'root'
})
export class AuthService {
    private supabase: SupabaseClient;
    private _currentUser: User | null = null;
    private _role: UserRole | null = null;

    constructor(private http: HttpClient) {
        this.supabase = createClient(environment.supabaseUrl, environment.supabaseAnonKey);
    }

    get currentUser(): User | null { return this._currentUser; }
    get role(): UserRole | null { return this._role; }

    async login(email: string, senha: string, perfilSelecionado: UserRole): Promise<UserRole> {
        const { data, error } = await this.supabase.auth.signInWithPassword({
            email,
            password: senha,
        });

        if (error) throw error;
        if (!data.session) throw new Error('Sessão não criada.');

        this._currentUser = data.user;
        localStorage.setItem('jwt_token', data.session.access_token);

        // Admin é identificado pelo perfil selecionado (não tem tabela própria)
        if (perfilSelecionado === 'admin') {
            this._role = 'admin';
            localStorage.setItem('user_role', 'admin');
            return 'admin';
        }

        // Para professor e aluno, confirma pelo perfil selecionado
        // (o admin criou o usuário com o tipo correto)
        this._role = perfilSelecionado;
        localStorage.setItem('user_role', perfilSelecionado);
        localStorage.setItem('user_id', data.user.id);
        return perfilSelecionado;
    }

    restoreSession(): boolean {
        const token = localStorage.getItem('jwt_token');
        const role = localStorage.getItem('user_role') as UserRole | null;
        if (token && role) {
            this._role = role;
            return true;
        }
        return false;
    }

    logout() {
        this._currentUser = null;
        this._role = null;
        localStorage.removeItem('jwt_token');
        localStorage.removeItem('user_role');
        localStorage.removeItem('user_id');
        this.supabase.auth.signOut();
    }

    getToken(): string | null {
        return localStorage.getItem('jwt_token');
    }

    getAuthId(): string | null {
        return localStorage.getItem('user_id');
    }
}
