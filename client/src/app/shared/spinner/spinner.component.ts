import { Component } from '@angular/core';
import { LoaderService } from '../../services/loader.service';

@Component({
  selector: 'spinner',
  templateUrl: './spinner.component.html'
})
export class SpinnerComponent {
  constructor(public loader: LoaderService) { }
}