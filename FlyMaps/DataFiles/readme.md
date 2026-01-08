# Overview
You have two hours to build a .NET 8 application that parses two data sources (a CSV file and a JSON file), persists the resulting data, and displays an interactive UI. 

The goal is a working solution that lets a user view gene details, including descriptions and aliases.

If at any point you have questions about the assignment, **please ask us**.

**IMPORTANT**: You may NOT use any automated coding tools (including GitHub Copilot, Windsurf, etc.) or AI (ChatGPT, etc.) in this assignment.

## Biological Context: Genes & Aliases
### Gene Symbol

Each gene is typically assigned a short name (e.g., A2M, TP53, EGFR, CDK2). This name is called the gene symbol, and is sometimes regulated by official nomenclature committees.

### Gene Description
A gene description is a short textual summary of the gene’s function or role (e.g., “Protein kinase involved in cell cycle regulation…”). Different databases might each provide their own description of the same gene.

### Aliases (a.k.a. Synonyms, other names for the gene)
Genes may also have synonyms because: (1) Multiple researchers discovered them independently and gave different names, and (2) Different functions or contexts of those genes may have led to multiple names. For instance, TP53 might also be called p53. These extra names are called “aliases” (or synonyms). 

## Data Sources
There are 4 sources of data that we will use in this test. They are:
* NCBI Gene
* Ensembl
* HGNC
* GeneCards

Each of those data sources have their own website, and have a way of identifying a "gene" entry with their own unique identifiers. For examples:

* NCBI - https://www.ncbi.nlm.nih.gov/gene/[DatabaseIdentifier]
  * Eg: https://www.ncbi.nlm.nih.gov/gene/102724660

* Ensembl - https://useast.ensembl.org/Homo_sapiens/Gene/Summary?db=core;g=[DatabaseIdentifier]
  * Eg: https://useast.ensembl.org/Homo_sapiens/Gene/Summary?db=core;g=ENSG00000133703

* HGNC - https://www.genenames.org/data/gene-symbol-report/#!/hgnc_id/HGNC:[DatabaseIdentifier]
  * Eg: https://www.genenames.org/data/gene-symbol-report/#!/hgnc_id/HGNC:55641

* GeneCards - No link required

When creating links to the various data sources in the website you're going to be creating, make sure to use the templates above.

## Data Files
The files provided to you will be:
- `gene_aliases.csv` - contains basic information about genes, including: Gene symbol, Aliases, Source database identifiers and Source database name. Importantly, you will be getting the database identifier here which is used for the links later (see links above in Data Sources)

- `gene_descriptions.json` - contains descriptions of genes from various sources. You should ignore any descriptions from sources that aren't Ensembl, NCBI Gene, HGNC or GeneCards.

## Task & Requirements

### Data Sources
Create an appropriate relational data model based on the data provided to you. 

### Persistence
Store the combined data in SQL Server using any .NET approach (EF, Dapper, NHibernate, etc.).

### UI with Client-Side Interactivity

Build at least one page that displays a selected gene (or search results) with the following layout:
- Gene Symbol in an `<h1 />` element.

- A _descriptions_ section, which lists all the descriptions for that gene. Each description must be followed with a link to the source from which the description was received.

- An _aliases_ section listing alternative names, also with links to original sources. Since you may have the same alias from multiple sources, you should consider displaying it once with multiple superscripts. eg: TP53 <sup><a href="#">1</a>, <a href="#">2</a></sup>

**IMPORTANT**: You must include JavaScript or TypeScript interactivity so the user can do something beyond static HTML. For instance, a search field, or clickable gene entries that update the display on the same page, etc.

## Deliverables

- Project: A .NET 8+ solution that can be run locally with minimal configuration.

- Data Parsing: Code that reads `genes_aliases.csv` and `gene_descriptions.json`, merges them, and inserts/updates the data in your chosen database.

- UI: At least one page with a working JS/TS-based interaction that displays a gene’s symbol, descriptions, and aliases with source links.

## Evaluation Criteria

- Correctness: Is the data properly merged? Are the aliases assigned to the right genes?
- Persistence & DB Structure: Did you set up a table schema that makes sense?
- UI Functionality: Do we see the gene symbol, descriptions, aliases, and links in a workable front-end? Is there some basic interactivity with JavaScript or TypeScript?
- Readability: Is your code reasonably organized or commented?
- Time Management: Did you focus on the critical elements (data ingestion, display, references) rather than perfect architecture?

## Tips

- Spend some time understanding the data model by looking at the CSV and the Json files, to come up with the right data structure to model.
- If you wish to make compromises while coding, i.e. things you do quickly but would do differently on a real-life project, that is fine, as long as you communicate it to us.