export interface IFilterConfig {
  reportId: string,
  tableName: string,
  column: {
    name: string,
    values: string[],
    isMultiSelect: boolean
  }
}