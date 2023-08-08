import jsPDF from 'jspdf';
import html2canvas from 'html2canvas';
import { Component, OnInit } from '@angular/core';
import { CV, EducationCV, ExperienceCV, ExportCVClient, IUserDto, ProjectCV, SkillCV, UserCV } from 'src/app/web-api-client';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-export-cv',
  templateUrl: './export-cv.component.html',
  styleUrls: ['./export-cv.component.css']
})
export class ExportCvComponent implements OnInit {

  cv: CV;
  user: UserCV;
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
            this.experiences = data.educationCVs;
            this.educations = data.educationCVs;
            this.skills = data.skillCVs;
            this.projects = data.projectCVs;
            this.user = data.user
          }
        }
      }
    )
  }

  

  public openPDF(): void {
    let DATA: any = document.getElementById('htmlData');
    html2canvas(DATA, { backgroundColor: 'white' }).then((canvas) => {
      let fileWidth = 150;
      let fileHeight = (canvas.height * fileWidth) / canvas.width;
      const FILEURI = canvas.toDataURL('image/png');
      let PDF = new jsPDF('p', 'mm', 'a4', false);
      let position = 0;
      PDF.addImage(FILEURI, 'PNG', 0, position, fileWidth, fileHeight);
      PDF.save('angular-demo.pdf');
    });
  }

}
