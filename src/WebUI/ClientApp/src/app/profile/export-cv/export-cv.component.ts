import jsPDF from 'jspdf';
import html2canvas from 'html2canvas';
import { Component, OnInit } from '@angular/core';
import { CV, EducationCV, ExperienceCV, ExportCVClient, IUserCV, IUserDto, ProjectCV, SkillCV, UserCV } from 'src/app/web-api-client';
import { ActivatedRoute } from '@angular/router';
import * as moment from 'moment';

@Component({
  selector: 'app-export-cv',
  templateUrl: './export-cv.component.html',
  styleUrls: ['./export-cv.component.css']
})
export class ExportCvComponent implements OnInit {

  cv: CV;
  user: IUserCV = {
    firstName: '',
    email: '',
    profileImage:'',
    lastName:'',
    summary:'',
    dateOfBirth: undefined,
    address: '',
  };
  userId: number;
  educations: EducationCV[] = [];
  skills: SkillCV[] = [];
  projects: ProjectCV[] = [];
  experiences: ExperienceCV[] = [];


  constructor(private exportCVClient: ExportCVClient, private router: ActivatedRoute,
  ) { }

  ngOnInit(): void {
    this.userId = parseInt(this.router.snapshot.paramMap.get('id'));
    this.exportCVClient.exportCV(this.userId).subscribe(
      {
        next: data => {
          this.cv = data;
          if (data) {
            this.experiences = data.experienceCVs;
            this.educations = data.educationCVs;
            this.skills = data.skillCVs;
            this.projects = data.projectCVs;
            this.user = data.user
          }
        }
      }
    )
  }
  getFormattedDate(created: Date): string {
    const currentDate = moment();
    const jobDate = moment(created);

    return moment(created).format('MMM DD, YYYY');
  }
  

  // public openPDF(): void {
  //   let DATA: any = document.getElementById('htmlData');
  //   html2canvas(DATA, { backgroundColor: 'white' }).then((canvas) => {
  //     let fileWidth = 150;
  //     let fileHeight = (canvas.height * fileWidth) / canvas.width;
  //     const FILEURI = canvas.toDataURL('image/png');
  //     let PDF = new jsPDF('p', 'mm', 'a4', false);
  //     let position = 0;
  //     PDF.addImage(FILEURI, 'PNG', 0, position, fileWidth, fileHeight);
  //     PDF.save('angular-demo.pdf');
  //   });
  // }

  getPDF() {
    const data = document.getElementById('htmlData');
    html2canvas(data).then((canvas: any) => {
      const imgWidth = 150;
      const pageHeight = 250;
      const imgHeight = (canvas.height * imgWidth) / canvas.width;
      let heightLeft = imgHeight;
      let position = 0;
      heightLeft -= pageHeight;
      const doc = new jsPDF('p', 'mm');
      doc.addImage(canvas, 'PNG', 0, position, imgWidth, imgHeight, '', 'FAST');
      while (heightLeft >= 0) {
        position = heightLeft - imgHeight;
        doc.addPage();
        doc.addImage(canvas, 'PNG', 0, position, imgWidth, imgHeight, '', 'FAST');
        heightLeft -= pageHeight;
      }
      doc.save('Downld.pdf');
    });
  
   }
}
