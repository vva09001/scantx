import React, { Component } from "react";
import { connect } from "react-redux";
import { CircularProgress } from "components/uielements/progress";
import Form from "./Form";
import EditForm from "./EditForm";
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
      toggleEdit: false,
      editAlert: false,
      delete: false,
      deleteMulti: false,
      deleteId: null,
      multiId: [],
      params: {}
    };
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
            Delete|
          </Link>
          <Link
            component="button"
            variant="body2"
            onClick={() => this.onToggleEditForm(true, item)}
          >
            Edit
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

  onToggleEditForm = (status, item) => {
    if (status === true) {
      this.setState({
        toggleEdit: status,
        params: item
      });
    }
    else {
      this.setState({
        toggleEdit: status
      });
    }
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
        multiId: [...this.state.multiId, id]
      });
    } else {
      this.setState({
        multiId: this.state.multiId.filter(item => item !== id)
      });
    }
  };

  edit = (params, success, fail) => {
    this.props.editScanData(params, success, fail);
    this.setState({
      toggleEdit: false,
      params: {},
    });
  };

  add = (params, success, fail) => {
    this.props.addScanData(params, success, fail);
    this.setState({
      toggle: false
    });
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
    this.props.deleteMultiScanData(
      this.state.multiId,
      this.onSuccess,
      this.onSuccess
    );
    this.setState({
      deleteMulti: false,
      multiId: []
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
                onClick={() => this.onToggleDeleteMulti(true)}
              >
                Delete selected
              </Button>
            </Grid>
          </Papersheet>
          {/* Add Form */}
          <Form
            onToggle={this.onToggleForm}
            onSubmit={this.add}
            status={this.state.toggle}
          />
          {/* Edit Form */}
          <EditForm
            onToggle={this.onToggleEditForm}
            onSubmit={this.edit}
            status={this.state.toggleEdit}
            params={this.state.params}
          />
          {/* Delete Alert */}
          <DeleteAlert
            status={this.state.delete}
            onSubmit={this.delete}
            onClose={this.onToggleDelete}
          />
          {/* Delete Multi Alert */}
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
  deleteMultiScanData: scanDataActions.deleteMulti,
  editScanData: scanDataActions.edit,
  addScanData: scanDataActions.add
};
export default connect(
  mapSateToProps,
  mapDispatchToProps
)(ScanData);
