import React, { Component } from "react";
import { connect } from "react-redux";
import { CircularProgress } from "components/uielements/progress";
import Form from "./Form";
import DeleteAlert from "./Alert";
import LayoutWrapper from "components/utility/layoutWrapper";
import Papersheet from "components/utility/papersheet";
import { FullColumn } from "components/utility/rowColumn";
import Button from "components/uielements/button/index.js";
import Grid from "components/uielements/grid";
import Table from "components/uielements/table";
import Checkbox from "components/uielements/checkbox";
import {
  TableBody,
  TableCell,
  TableHead,
  TableRow
} from "components/uielements/table";
import FormControl from "@material-ui/core/FormControl";
import Select from "@material-ui/core/Select";
import OutlinedInput from "@material-ui/core/OutlinedInput";
import Link from "@material-ui/core/Link";
import { scanDataActions } from "redux/actions";
import { Date, Time } from "helpers/moment";
import _ from "lodash";
import "styles/style.css";

class ScanData extends Component {
  constructor(props) {
    super(props);
    this.state = {
      loading: true,
      toggle: false,
      delete: false,
      deleteMulti: false,
      deleteId: null,
      deleteMultiId: [],
      params: {}
    };
    this.handleCheck = this.handleCheck.bind(this);
    this.deleteMulti = this.deleteMulti.bind(this);
  }
  componentDidMount() {
    this.props.getScanData(this.onSuccess, this.onSuccess);
  }
  onSuccess = () => {
    this.setState({
      loading: false
    });
  };
  renderData = () => {
    return _.map(this.props.datas, item => (
      <TableRow key={item.scanId}>
        <TableCell padding="checkbox">
          <Checkbox onChange={e => this.handleCheck(e, item.scanId)} />
        </TableCell>
        <TableCell>{Date(item.createdOn)}</TableCell>
        <TableCell>{Time(item.createdOn)}</TableCell>
        <TableCell>{item.dataType}</TableCell>
        <TableCell>{item.status}</TableCell>
        <TableCell>
          <Link
            component="button"
            variant="body2"
            onClick={() => this.onToggleDelete(true, item.scanId)}
          >
            Delete
          </Link>
        </TableCell>
      </TableRow>
    ));
  };

  onToggleForm = (status, params = {}) => {
    this.setState({
      toggle: status,
      params: params
    });
  };

  onToggleDelete = (status, deleteId) => {
    this.setState({
      delete: status,
      deleteId: deleteId
    });
  };

  onToggleDeleteMulti = status => {
    this.setState({
      deleteMulti: status
    });
  };

  handleCheck = (event, id) => {
    if (event.target.checked) {
      this.setState({
        deleteMultiId: [...this.state.deleteMultiId, id]
      });
    }
    else {
      this.setState({
        deleteMultiId: this.state.deleteMultiId.filter(item => item !== id)
      });
    }
  };

  delete = () => {
    this.props.deleteScanData(
      this.state.deleteId,
      this.onSuccess,
      this.onSuccess
    );
    this.setState({
      delete: false,
      deleteId: null
    });
  };

  deleteMulti = () => {
    console.log(this.state);
    this.props.deleteMultiScanData(
      this.state.deleteMultiId,
      this.onSuccess,
      this.onSuccess
    );
    this.setState({
      deleteMulti: false,
      deleteMultiId: []
    });
  };

  render() {
    if (this.state.loading) {
      return <CircularProgress />;
    }
    return (
      <LayoutWrapper>
        <FullColumn>
          <Papersheet title="Scan Data">
            <Grid container justify="flex-end">
              <Grid item xs={3}>
                <FormControl variant="outlined" margin="dense" fullWidth>
                  <Select native input={<OutlinedInput />}>
                    <option value="">None</option>
                    <option value="PC_M3">PC_M3</option>
                  </Select>
                </FormControl>
              </Grid>
            </Grid>
            <Table>
              <TableHead>
                <TableRow>
                  <TableCell padding="checkbox">
                    <Checkbox />
                  </TableCell>
                  <TableCell>Date</TableCell>
                  <TableCell>Time</TableCell>
                  <TableCell>Data</TableCell>
                  <TableCell>Status</TableCell>
                  <TableCell></TableCell>
                </TableRow>
              </TableHead>
              <TableBody>{this.renderData()}</TableBody>
            </Table>
            <Grid
              container
              direction="row"
              justify="center"
              alignItems="center"
            >
              <Button
                className="buttonStyles"
                variant="contained"
                color="primary"
                onClick={() => this.onToggleForm(true)}
              >
                Add new scan data
              </Button>
              <Button
                className="buttonStyles"
                variant="contained"
                color="primary"
              >
                Copy selected
              </Button>
              <Button
                className="buttonStyles"
                variant="contained"
                color="primary"
              >
                Edit selected
              </Button>
              <Button
                className="buttonStyles"
                variant="contained"
                color="primary"
                onClick={() => this.onToggleDeleteMulti(true)}
              >
                Delete selected
              </Button>
            </Grid>
          </Papersheet>
          <Form
            onToggle={this.onToggleForm}
            status={this.state.toggle}
            params={this.state.params}
          />
          <DeleteAlert
            status={this.state.delete}
            onSubmit={this.delete}
            onClose={this.onToggleDelete}
          />
          <DeleteAlert
            status={this.state.deleteMulti}
            onSubmit={this.deleteMulti}
            onClose={this.onToggleDeleteMulti}
          />
        </FullColumn>
      </LayoutWrapper>
    );
  }
}
const mapSateToProps = state => {
  return {
    datas: state.ScanData.list
  };
};

const mapDispatchToProps = {
  getScanData: scanDataActions.getScanData,
  deleteScanData: scanDataActions.delete,
  deleteMultiScanData: scanDataActions.deleteMulti
};
export default connect(
  mapSateToProps,
  mapDispatchToProps
)(ScanData);
