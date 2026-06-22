import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { createClient, SupabaseClient, User } from '@supabase/supabase-js';
import { environment } from '../../../environments/environment';

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

        if (perfilSelecionado === 'admin') {
            if (email !== 'admin@oficina.com') {
                this.supabase.auth.signOut();
                throw new Error('Acesso negado: Este email não tem privilégios de administrador.');
            }
            this._currentUser = data.user;
            this._role = 'admin';
            localStorage.setItem('jwt_token', data.session.access_token);
            localStorage.setItem('user_role', 'admin');
            return 'admin';
        }

        this._currentUser = data.user;
        localStorage.setItem('jwt_token', data.session.access_token);
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
