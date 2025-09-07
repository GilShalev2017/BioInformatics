// import { Component, AfterViewInit, ElementRef, ViewChild } from '@angular/core';
// import { CommonModule } from '@angular/common';
// import * as d3 from 'd3';
// import { BioService } from '../services/bio.service';

// @Component({
//   selector: 'app-relationships',
//   standalone: true,
//   imports: [CommonModule],
//   templateUrl: './relationships.component.html',
//   styleUrls: ['./relationships.component.scss']
// })
// export class RelationshipsGraphComponent implements AfterViewInit {
//   @ViewChild('graphContainer', { static: true }) private graphContainer!: ElementRef;

//   constructor(private bioService: BioService) {}

//   ngAfterViewInit() {
//     const element = this.graphContainer.nativeElement;
//     setTimeout(() => this.loadData(element), 0);
//   }

//   private loadData(element: HTMLElement) {
//     const width = element.clientWidth || 800;
//     const height = element.clientHeight || 600;

//     this.bioService.getRelationships().subscribe(data => {
//       const nodesMap = new Map<string, any>();
//       const links: any[] = [];

//       // Genes
//       data.Genes.forEach((g: any) => {
//         if (!nodesMap.has(g.GeneID)) {
//           nodesMap.set(g.GeneID, { id: g.GeneID, label: g.GeneName, group: 'gene' });
//         }

//         g.RelatedDiseases.forEach((d: any) => {
//           if (!nodesMap.has(d.DiseaseID)) {
//             nodesMap.set(d.DiseaseID, { id: d.DiseaseID, label: d.DiseaseName, group: 'disease' });
//           }
//           links.push({ source: g.GeneID, target: d.DiseaseID, type: 'gene-disease' });
//         });

//         g.TargetedByDrugs.forEach((dr: any) => {
//           if (!nodesMap.has(dr.DrugId)) {
//             nodesMap.set(dr.DrugId, { id: dr.DrugId, label: dr.DrugName, group: 'drug' });
//           }
//           links.push({ source: dr.DrugId, target: g.GeneID, type: 'drug-gene' });
//         });
//       });

//       // Ensure all diseases are included
//       data.Diseases.forEach((d: any) => {
//         if (!nodesMap.has(d.DiseaseID)) {
//           nodesMap.set(d.DiseaseID, { id: d.DiseaseID, label: d.DiseaseName, group: 'disease' });
//         }
//       });

//       // Ensure all drugs are included
//       data.Drugs.forEach((dr: any) => {
//         if (!nodesMap.has(dr.DrugId)) {
//           nodesMap.set(dr.DrugId, { id: dr.DrugId, label: dr.DrugName, group: 'drug' });
//         }
//       });

//       const nodes = Array.from(nodesMap.values());
//       this.createGraph(element, nodes, links, width, height);
//     });
//   }

//   private createGraph(element: HTMLElement, nodes: any[], links: any[], width: number, height: number) {
//     const svg = d3.select(element).append('svg')
//       .attr('width', width)
//       .attr('height', height);

//     // container for zooming & panning
//     const container = svg.append('g');

//     // enable zoom & pan
//     svg.call(
//       d3.zoom<SVGSVGElement, unknown>()
//         .scaleExtent([0.1, 5]) // min and max zoom
//         .on('zoom', (event) => {
//           container.attr('transform', event.transform);
//         })
//     );

//     const color = (group: string) => {
//       switch (group) {
//         case 'gene': return '#1f77b4';
//         case 'disease': return '#d62728';
//         case 'drug': return '#2ca02c';
//         default: return '#ccc';
//       }
//     };

//     const simulation = d3.forceSimulation(nodes)
//       .force('link', d3.forceLink(links).id((d: any) => d.id).distance(120))
//       .force('charge', d3.forceManyBody().strength(-300))
//       .force('center', d3.forceCenter(width / 2, height / 2));

//     const link = container.append('g')
//       .attr('stroke', '#999')
//       .attr('stroke-opacity', 0.6)
//       .selectAll('line')
//       .data(links)
//       .enter().append('line')
//       .attr('stroke-width', 1.5);

//     const node = container.append('g')
//       .attr('stroke', '#fff')
//       .attr('stroke-width', 1.5)
//       .selectAll('circle')
//       .data(nodes)
//       .enter().append('circle')
//       .attr('r', 10)
//       .attr('fill', (d: any) => color(d.group))
//       .call(d3.drag<SVGCircleElement, any>()
//         .on('start', (event, d: any) => {
//           if (!event.active) simulation.alphaTarget(0.3).restart();
//           d.fx = d.x;
//           d.fy = d.y;
//         })
//         .on('drag', (event, d: any) => {
//           d.fx = event.x;
//           d.fy = event.y;
//         })
//         .on('end', (event, d: any) => {
//           if (!event.active) simulation.alphaTarget(0);
//           d.fx = null;
//           d.fy = null;
//         })
//       );

//     const labels = container.append('g')
//       .selectAll('text')
//       .data(nodes)
//       .enter().append('text')
//       .text((d: any) => d.label)
//       .attr('font-size', 10)
//       .attr('dx', 12)
//       .attr('dy', '.35em');

//     simulation.on('tick', () => {
//       link
//         .attr('x1', (d: any) => d.source.x)
//         .attr('y1', (d: any) => d.source.y)
//         .attr('x2', (d: any) => d.target.x)
//         .attr('y2', (d: any) => d.target.y);

//       node
//         .attr('cx', (d: any) => d.x)
//         .attr('cy', (d: any) => d.y);

//       labels
//         .attr('x', (d: any) => d.x)
//         .attr('y', (d: any) => d.y);
//     });
//   }
// }

import { Component, AfterViewInit, ElementRef, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import * as d3 from 'd3';
import { BioService } from '../services/bio.service';

@Component({
  selector: 'app-relationships',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './relationships.component.html',
  styleUrls: ['./relationships.component.scss']
})
export class RelationshipsGraphComponent implements AfterViewInit {
  @ViewChild('graphContainer', { static: true }) private graphContainer!: ElementRef;

  private svg!: d3.Selection<SVGSVGElement, unknown, null, undefined>;
  private container!: d3.Selection<SVGGElement, unknown, null, undefined>;
  private zoom!: d3.ZoomBehavior<SVGSVGElement, unknown>;
  private width = 1600;//800;
  private height = 1200;//600;
  private nodes: any[] = [];
  private links: any[] = [];

  constructor(private bioService: BioService) { }

  ngAfterViewInit() {
    const element = this.graphContainer.nativeElement;
    setTimeout(() => this.loadData(element), 0);
  }

  private loadData(element: HTMLElement) {
    this.width = element.clientWidth || 1600;//800;
    this.height = element.clientHeight || 1200;//600;

    this.bioService.getRelationships().subscribe(data => {
      const nodesMap = new Map<string, any>();
      const links: any[] = [];

      // Genes
      data.Genes.forEach((g: any) => {
        if (!nodesMap.has(g.GeneID)) {
          nodesMap.set(g.GeneID, { id: g.GeneID, label: g.GeneName, group: 'gene' });
        }

        g.RelatedDiseases.forEach((d: any) => {
          if (!nodesMap.has(d.DiseaseID)) {
            nodesMap.set(d.DiseaseID, { id: d.DiseaseID, label: d.DiseaseName, group: 'disease' });
          }
          links.push({ source: g.GeneID, target: d.DiseaseID, type: 'gene-disease' });
        });

        g.TargetedByDrugs.forEach((dr: any) => {
          if (!nodesMap.has(dr.DrugId)) {
            nodesMap.set(dr.DrugId, { id: dr.DrugId, label: dr.DrugName, group: 'drug' });
          }
          links.push({ source: dr.DrugId, target: g.GeneID, type: 'drug-gene' });
        });
      });

      // Ensure all diseases
      data.Diseases.forEach((d: any) => {
        if (!nodesMap.has(d.DiseaseID)) {
          nodesMap.set(d.DiseaseID, { id: d.DiseaseID, label: d.DiseaseName, group: 'disease' });
        }
      });

      // Ensure all drugs
      data.Drugs.forEach((dr: any) => {
        if (!nodesMap.has(dr.DrugId)) {
          nodesMap.set(dr.DrugId, { id: dr.DrugId, label: dr.DrugName, group: 'drug' });
        }
      });

      this.nodes = Array.from(nodesMap.values());
      this.links = links;

      this.createGraph(element);
    });
  }

  private createGraph(element: HTMLElement) {
    this.svg = d3.select(element).append('svg')
      .attr('width', this.width)
      .attr('height', this.height);

    this.container = this.svg.append('g');

    this.zoom = d3.zoom<SVGSVGElement, unknown>()
      .scaleExtent([0.1, 5])
      .on('zoom', (event) => {
        this.container.attr('transform', event.transform);
      });

    this.svg.call(this.zoom);

    const color = (group: string) => {
      switch (group) {
        case 'gene': return '#1f77b4';
        case 'disease': return '#d62728';
        case 'drug': return '#2ca02c';
        default: return '#ccc';
      }
    };

    const simulation = d3.forceSimulation(this.nodes)
      .force('link', d3.forceLink(this.links).id((d: any) => d.id).distance(120))
      .force('charge', d3.forceManyBody().strength(-300))
      .force('center', d3.forceCenter(this.width / 2, this.height / 2));

    const link = this.container.append('g')
      .attr('stroke', '#999')
      .attr('stroke-opacity', 0.6)
      .selectAll('line')
      .data(this.links)
      .enter().append('line')
      .attr('stroke-width', 1.5);

    const node = this.container.append('g')
      .attr('stroke', '#fff')
      .attr('stroke-width', 1.5)
      .selectAll('circle')
      .data(this.nodes)
      .enter().append('circle')
      .attr('r', 10)
      .attr('fill', (d: any) => color(d.group))
      .call(d3.drag<SVGCircleElement, any>()
        .on('start', (event, d: any) => {
          if (!event.active) simulation.alphaTarget(0.3).restart();
          d.fx = d.x;
          d.fy = d.y;
        })
        .on('drag', (event, d: any) => {
          d.fx = event.x;
          d.fy = event.y;
        })
        .on('end', (event, d: any) => {
          if (!event.active) simulation.alphaTarget(0);
          d.fx = null;
          d.fy = null;
        })
      );

    const labels = this.container.append('g')
      .selectAll('text')
      .data(this.nodes)
      .enter().append('text')
      .text((d: any) => d.label)
      .attr('font-size', 10)
      .attr('dx', 12)
      .attr('dy', '.35em');

    simulation.on('tick', () => {
      link
        .attr('x1', (d: any) => d.source.x)
        .attr('y1', (d: any) => d.source.y)
        .attr('x2', (d: any) => d.target.x)
        .attr('y2', (d: any) => d.target.y);

      node
        .attr('cx', (d: any) => d.x)
        .attr('cy', (d: any) => d.y);

      labels
        .attr('x', (d: any) => d.x)
        .attr('y', (d: any) => d.y);
    });
  }

  fitToScreen() {
    if (!this.svg || this.nodes.length === 0) return;

    const bounds = this.container.node()!.getBBox();
    const fullWidth = this.width;
    const fullHeight = this.height;

    const scale = Math.min(
      fullWidth / bounds.width,
      fullHeight / bounds.height,
      5
    ) * 0.9; // padding

    const translateX = fullWidth / 2 - (bounds.x + bounds.width / 2) * scale;
    const translateY = fullHeight / 2 - (bounds.y + bounds.height / 2) * scale;

    const transform = d3.zoomIdentity
      .translate(translateX, translateY)
      .scale(scale);

    this.svg.transition().duration(750).call(this.zoom.transform, transform);
  }
}
