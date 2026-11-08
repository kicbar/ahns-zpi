import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { Student, StudentRequest } from './student';
import { StudentService } from './student.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './app.component.html'
})
export class AppComponent {
  private readonly studentService = inject(StudentService);

  readonly students = signal<Student[]>([]);
  formData: StudentRequest = this.createEmptyStudent();
  editingId: number | null = null;
  readonly isLoading = signal(false);
  readonly hasLoaded = signal(false);
  readonly isSaving = signal(false);
  readonly isDeleting = signal(false);
  readonly errorMessage = signal('');
  readonly successMessage = signal('');

  loadStudents(): void {
    this.isLoading.set(true);
    this.errorMessage.set('');
    this.studentService.getAll().subscribe({
      next: students => {
        this.students.set(students);
        this.hasLoaded.set(true);
        this.isLoading.set(false);
      },
      error: (error: HttpErrorResponse) => {
        this.setError(error);
        this.isLoading.set(false);
      }
    });
  }

  save(form: NgForm): void {
    if (this.isSaving()) return;

    this.isSaving.set(true);
    this.errorMessage.set('');
    this.successMessage.set('');
    const wasEditing = this.editingId !== null;
    const request = { ...this.formData };
    const operation = this.editingId === null
      ? this.studentService.create(request)
      : this.studentService.update(this.editingId, request);

    operation.subscribe({
      next: () => {
        this.successMessage.set(wasEditing ? 'Student updated. Click Fetch to reload the list.' : 'Student added. Click Fetch to reload the list.');
        this.cancelEdit(form);
        this.isSaving.set(false);
      },
      error: (error: HttpErrorResponse) => {
        this.setError(error);
        this.isSaving.set(false);
      }
    });
  }

  edit(student: Student): void {
    this.editingId = student.id;
    this.formData = {
      firstName: student.firstName,
      lastName: student.lastName,
      age: student.age,
      major: student.major
    };
    this.errorMessage.set('');
    this.successMessage.set('');
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }

  remove(student: Student): void {
    if (this.isDeleting() || !confirm(`Delete ${student.firstName} ${student.lastName}?`)) return;

    this.isDeleting.set(true);
    this.errorMessage.set('');
    this.successMessage.set('');
    this.studentService.delete(student.id).subscribe({
      next: () => {
        this.successMessage.set('Student deleted. Click Fetch to reload the list.');
        if (this.editingId === student.id) this.cancelEdit();
        this.isDeleting.set(false);
      },
      error: (error: HttpErrorResponse) => {
        this.setError(error);
        this.isDeleting.set(false);
      }
    });
  }

  cancelEdit(form?: NgForm): void {
    this.editingId = null;
    this.formData = this.createEmptyStudent();
    form?.resetForm(this.createEmptyStudent());
  }

  private createEmptyStudent(): StudentRequest {
    return { firstName: '', lastName: '', age: 20, major: '' };
  }

  private setError(error: HttpErrorResponse): void {
    this.errorMessage.set(`Request failed (HTTP ${error.status || 'network error'}). Check the backend at http://localhost:5080.`);
  }
}
