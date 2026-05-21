/**
 * Exemple complet d'intégration Angular avec JWT et Confirmation Dialog
 * Pour utiliser ces fichiers, assurez-vous d'avoir:
 * - npm install @angular/material
 * - Importé MatDialogModule et BrowserAnimationsModule dans app.module.ts
 */

// ============================================
// 1. auth.service.ts
// ============================================

import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { tap, catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';

export interface TokenResponse {
  token: string;
  expiresIn: string;
  id: number;
  userType: 'Admin' | 'Societe';
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'http://localhost:5000/api';
  private currentUserSubject = new BehaviorSubject<any>(this.getUserFromStorage());
  public currentUser$ = this.currentUserSubject.asObservable();

  constructor(private http: HttpClient) {
    this.checkTokenExpiration();
  }

  loginAdmin(email: string, password: string): Observable<TokenResponse> {
    return this.http.post<TokenResponse>(`${this.apiUrl}/admin/login`, { email, password })
      .pipe(
        tap(response => this.handleLoginSuccess(response)),
        catchError(error => this.handleError(error))
      );
  }

  loginSociete(email: string, password: string): Observable<TokenResponse> {
    return this.http.post<TokenResponse>(`${this.apiUrl}/societe/login`, { email, password })
      .pipe(
        tap(response => this.handleLoginSuccess(response)),
        catchError(error => this.handleError(error))
      );
  }

  private handleLoginSuccess(response: TokenResponse): void {
    localStorage.setItem('token', response.token);
    localStorage.setItem('userType', response.userType);
    localStorage.setItem('userId', response.id.toString());
    localStorage.setItem('expiresIn', response.expiresIn);
    this.currentUserSubject.next({
      id: response.id,
      userType: response.userType
    });
  }

  private getUserFromStorage(): any {
    const token = localStorage.getItem('token');
    if (!token) return null;
    return {
      id: localStorage.getItem('userId'),
      userType: localStorage.getItem('userType')
    };
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  getUserType(): 'Admin' | 'Societe' | null {
    return localStorage.getItem('userType') as any;
  }

  getUserId(): number | null {
    const id = localStorage.getItem('userId');
    return id ? parseInt(id) : null;
  }

  isAdmin(): boolean {
    return this.getUserType() === 'Admin';
  }

  isSociete(): boolean {
    return this.getUserType() === 'Societe';
  }

  logout(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('userType');
    localStorage.removeItem('userId');
    localStorage.removeItem('expiresIn');
    this.currentUserSubject.next(null);
  }

  isAuthenticated(): boolean {
    return !!this.getToken();
  }

  private checkTokenExpiration(): void {
    setInterval(() => {
      const expiresIn = localStorage.getItem('expiresIn');
      if (expiresIn && new Date(expiresIn) < new Date()) {
        this.logout();
      }
    }, 60000); // Check every minute
  }

  private handleError(error: any): Observable<never> {
    console.error('Auth error:', error);
    return throwError(() => new Error(error.error?.message || 'Authentication failed'));
  }
}

// ============================================
// 2. jwt.interceptor.ts
// ============================================

import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AuthService } from './auth.service';
import { Router } from '@angular/router';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
  constructor(private authService: AuthService, private router: Router) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const token = this.authService.getToken();
    
    if (token) {
      req = req.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`
        }
      });
    }

    return next.handle(req).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 401) {
          this.authService.logout();
          this.router.navigate(['/login']);
        }
        return throwError(() => error);
      })
    );
  }
}

// ============================================
// 3. confirm-dialog.component.ts
// ============================================

import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

export interface ConfirmDialogData {
  title: string;
  message: string;
  confirmText?: string;
  cancelText?: string;
  isDangerous?: boolean; // True for delete operations (red button)
}

@Component({
  selector: 'app-confirm-dialog',
  template: `
    <h2 mat-dialog-title>{{ data.title }}</h2>
    <mat-dialog-content>
      <p>{{ data.message }}</p>
    </mat-dialog-content>
    <mat-dialog-actions align="end">
      <button mat-button (click)="onCancel()">
        {{ data.cancelText || 'Annuler' }}
      </button>
      <button 
        mat-raised-button 
        [color]="data.isDangerous ? 'warn' : 'primary'" 
        (click)="onConfirm()">
        {{ data.confirmText || 'Confirmer' }}
      </button>
    </mat-dialog-actions>
  `
})
export class ConfirmDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<ConfirmDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: ConfirmDialogData
  ) {}

  onCancel(): void {
    this.dialogRef.close(false);
  }

  onConfirm(): void {
    this.dialogRef.close(true);
  }
}

// ============================================
// 4. Exemple: ligne-financiere.component.ts
// ============================================

import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmDialogComponent } from './confirm-dialog.component';
import { AuthService } from './auth.service';

interface LigneFinanciere {
  id: number;
  montant: number;
  quantite: number;
  description: string;
}

@Component({
  selector: 'app-ligne-financiere',
  template: `
    <div class="container">
      <h2>Lignes Financières</h2>
      
      <table class="table">
        <thead>
          <tr>
            <th>ID</th>
            <th>Description</th>
            <th>Montant</th>
            <th>Quantité</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          <tr *ngFor="let ligne of lignes">
            <td>{{ ligne.id }}</td>
            <td>{{ ligne.description }}</td>
            <td>{{ ligne.montant }}</td>
            <td>{{ ligne.quantite }}</td>
            <td>
              <button (click)="editMontant(ligne)" class="btn-edit">Modifier Montant</button>
              <button 
                *ngIf="authService.isAdmin()" 
                (click)="deleteLigne(ligne.id)" 
                class="btn-delete">
                Supprimer
              </button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  `,
  styles: [`
    .container { padding: 20px; }
    table { width: 100%; border-collapse: collapse; }
    th, td { padding: 12px; text-align: left; border: 1px solid #ddd; }
    th { background-color: #f5f5f5; }
    .btn-edit { 
      background-color: #2196F3; 
      color: white; 
      padding: 8px 12px; 
      border: none; 
      border-radius: 4px; 
      cursor: pointer;
      margin-right: 5px;
    }
    .btn-delete { 
      background-color: #f44336; 
      color: white; 
      padding: 8px 12px; 
      border: none; 
      border-radius: 4px; 
      cursor: pointer;
    }
  `]
})
export class LigneFinanciereComponent implements OnInit {
  lignes: LigneFinanciere[] = [];
  private apiUrl = 'http://localhost:5000/api/lignefinanciere';

  constructor(
    private http: HttpClient,
    private dialog: MatDialog,
    public authService: AuthService
  ) {}

  ngOnInit(): void {
    this.loadLignes();
  }

  loadLignes(): void {
    this.http.get<LigneFinanciere[]>(this.apiUrl).subscribe(
      data => this.lignes = data,
      error => console.error('Error loading lignes:', error)
    );
  }

  editMontant(ligne: LigneFinanciere): void {
    const newMontant = prompt('Entrez le nouveau montant:', ligne.montant.toString());
    
    if (newMontant === null) return;

    const montantValue = parseFloat(newMontant);
    if (isNaN(montantValue)) {
      alert('Montant invalide');
      return;
    }

    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'Modification du Montant',
        message: `Êtes-vous sûr de vouloir changer le montant de ${ligne.montant} à ${montantValue}?`,
        confirmText: 'Modifier',
        cancelText: 'Annuler'
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.http.patch(`${this.apiUrl}/${ligne.id}/montant`, montantValue).subscribe(
          () => {
            ligne.montant = montantValue;
            alert('Montant mis à jour avec succès');
          },
          error => alert('Erreur: ' + (error.error?.message || error))
        );
      }
    });
  }

  deleteLigne(id: number): void {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'Suppression',
        message: 'Êtes-vous sûr de vouloir supprimer cette ligne? Cette action est irréversible.',
        confirmText: 'Supprimer',
        cancelText: 'Annuler',
        isDangerous: true
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.http.delete(`${this.apiUrl}/${id}`).subscribe(
          () => {
            this.lignes = this.lignes.filter(l => l.id !== id);
            alert('Ligne supprimée');
          },
          error => alert('Erreur: ' + (error.error?.message || error))
        );
      }
    });
  }
}

// ============================================
// 5. app.module.ts - Configuration
// ============================================

import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';

import { AppComponent } from './app.component';
import { JwtInterceptor } from './jwt.interceptor';
import { ConfirmDialogComponent } from './confirm-dialog.component';
import { LigneFinanciereComponent } from './ligne-financiere.component';

@NgModule({
  declarations: [
    AppComponent,
    ConfirmDialogComponent,
    LigneFinanciereComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    MatDialogModule,
    MatButtonModule
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: JwtInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule {}
