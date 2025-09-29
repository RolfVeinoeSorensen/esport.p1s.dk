import { ChangeDetectionStrategy, ChangeDetectorRef, Component, inject } from '@angular/core';
import { LoadingHandlerService } from '@shared/loading/loading-handler.service';

@Component({
  selector: 'app-loading',
  templateUrl: './loading.component.html',
  styleUrls: ['./loading.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [],
})
export class LoadingComponent {
  loadingHandler = inject(LoadingHandlerService);
  private cdRef = inject(ChangeDetectorRef);

  spinnerActive: boolean = true;
  constructor() {
    this.loadingHandler.showSpinner.subscribe(this.showSpinner.bind(this));
  }

  showSpinner = (state: boolean): void => {
    this.spinnerActive = state;
    this.cdRef.markForCheck();
  };
}
