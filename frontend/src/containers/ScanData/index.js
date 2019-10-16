import React, { Component } from "react";
import { connect } from "react-redux";
import { CircularProgress } from "components/uielements/progress";
import CopyToClipboard from "./CopyToClipboard";
import Form from "./Form";
import EditForm from "./EditForm";
import DeleteAlert from "./Alert";
import SelectAlert from "./SelectAlert";
import LayoutWrapper from "components/utility/layoutWrapper";
import Papersheet from "components/utility/papersheet";
import DeleteIcon from "@material-ui/icons/Delete";
import EditIcon from "@material-ui/icons/Edit";
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
import Paper from "@material-ui/core/Paper";
import InputBase from "@material-ui/core/InputBase";
import IconButton from "@material-ui/core/IconButton";
import SearchIcon from "@material-ui/icons/Search";
import TablePagination from "@material-ui/core/TablePagination";
import { scanDataActions } from "redux/actions";
import { Date, Time } from "helpers/moment";
import moment from "moment";
import { permission } from "helpers/user";
import _ from "lodash";
import "styles/style.css";

let time = null;

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
      selectAlert: false,
      params: {},
      page: 0,
      rowsPerPage: 20,
      key: ""
    };
  }
  componentDidMount() {
    this.props.getScanData(this.onSuccess, this.onSuccess);
    this.onSync();
  }
  componentDidUpdate(prevProps, prevState) {
    if (this.state.key !== prevState.key) {
      this.onSync();
    }
  }
  onSync = () => {
    if (this.state.key === "") {
      time = setInterval(async () => {
        await this.props.getScanData(this.onSuccess, this.onSuccess);
      }, 5000);
    }
    if (this.state.key !== "") {
      clearInterval(time);
    }
  };

  onSuccess = () => {
    this.setState({
      loading: false
    });
  };

  handleChangePage = (event, newPage) => {
    this.setState({
      page: newPage
    });
  };

  handleChangeRowsPerPage = event => {
    this.setState({
      rowsPerPage: +event.target.value,
      page: 0
    });
  };

  renderData = () => {
    const { page, rowsPerPage } = this.state;
    const { datas } = this.props;
    return datas
      .slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage)
      .map(item => (
        <TableRow key={item.scanId}>
          <TableCell padding="checkbox">
            <Checkbox
              onChange={e => this.handleCheck(e, item.scanId)}
              checked={_.includes(this.state.multiId, item.scanId)}
            />
          </TableCell>
          <TableCell>{item.stationName}</TableCell>
          <TableCell>{moment(item.createdOn).format("MM/DD/YYYY")}</TableCell>
          <TableCell>{Time(item.createdOn)}</TableCell>
          <TableCell>{item.payload}</TableCell>
          {item.status === 0 && <TableCell>Received</TableCell>}
          {item.status === 1 && <TableCell>Processed</TableCell>}
          {item.status === 2 && <TableCell>Failed</TableCell>}
          <TableCell>
            <span
              className="actions"
              onClick={() => this.onToggleDelete(true, item.scanId)}
            >
              <DeleteIcon />
            </span>
            <span
              className="actions"
              onClick={() => this.onToggleEditForm(true, item)}
            >
              <EditIcon />
            </span>
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
    } else {
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
    if (this.state.multiId.length <= 0) {
      this.setState({
        selectAlert: status
      });
    } else {
      this.setState({
        deleteMulti: status
      });
    }
  };

  onToggleSelectAlert = status => {
    this.setState({ selectAlert: status });
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

  onSelectedAll = (status = false) => {
    const { multiId } = this.state;
    const { datas } = this.props;

    if (multiId.length === datas.length || status) {
      this.setState({
        multiId: []
      });
    } else {
      this.setState({
        multiId: _.map(datas, item => {
          return item.scanId;
        })
      });
    }
  };

  edit = (params, success, fail) => {
    this.props.editScanData(params, success, fail);
    this.setState({
      toggleEdit: false,
      params: {}
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

  copyScanData = () => {
    let content = "";
    _.map(this.props.datas, item => {
      if (_.includes(this.state.multiId, item.scanId)) {
        content += Date(item.createdOn) + "\t";
        content += Time(item.createdOn) + "\t";
        content += item.payload + "\t";
        content += item.status + "\t";
        content += "\n";
      }
    });
    return content;
  };

  downloadScanData = () => {
    this.props.downloadScanData(
      this.props.profile.id,
      this.onSuccess,
      this.onSuccess
    );
  };

  search = e => {
    if (e.key === "Enter") {
      this.props.search(this.state.key);
    }
  };
  onSearch = () => {
    this.props.search(this.state.key);
  };

  render() {
    const { profile } = this.props;
    if (this.state.loading) {
      return <CircularProgress />;
    }
    return (
      <LayoutWrapper>
        <FullColumn>
          <Papersheet title="Scan Data">
            <Grid container justify="flex-end">
              <Paper>
                <IconButton aria-label="menu">{/* <MenuIcon /> */}</IconButton>
                <InputBase
                  onChange={e => this.setState({ key: e.target.value })}
                  onKeyDown={e => this.search(e)}
                  placeholder="Search"
                  inputProps={{ "aria-label": "Search" }}
                />
                <IconButton aria-label="search" onClick={this.onSearch}>
                  <SearchIcon />
                </IconButton>
              </Paper>
            </Grid>
            <Table>
              <TableHead>
                <TableRow>
                  <TableCell padding="checkbox">
                    <Checkbox
                      onChange={() => this.onSelectedAll()}
                      checked={
                        this.state.multiId.length &&
                        this.state.multiId.length === this.props.datas.length
                      }
                    />
                  </TableCell>
                  <TableCell>Station Name</TableCell>
                  <TableCell>Date</TableCell>
                  <TableCell>Time</TableCell>
                  <TableCell>Data</TableCell>
                  <TableCell>Status</TableCell>
                  <TableCell>Actions</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>{this.renderData()}</TableBody>
            </Table>
            <TablePagination
              component="div"
              rowsPerPageOptions={[10, 20, 30]}
              count={this.props.datas.length}
              rowsPerPage={this.state.rowsPerPage}
              page={this.state.page}
              backIconButtonProps={{
                "aria-label": "previous page"
              }}
              nextIconButtonProps={{
                "aria-label": "next page"
              }}
              onChangePage={this.handleChangePage}
              onChangeRowsPerPage={this.handleChangeRowsPerPage}
            />
            <Grid
              container
              direction="row"
              justify="center"
              alignItems="center"
            >
              {_.indexOf(permission.scanData.add, profile.roleID) !== -1 && (
                <Button
                  className="buttonStyles"
                  variant="contained"
                  color="primary"
                  size="small"
                  onClick={() => this.onToggleForm(true)}
                >
                  Add new scan data
                </Button>
              )}
              <CopyToClipboard>
                {({ copy }) => (
                  <Button
                    className="buttonStyles"
                    variant="contained"
                    color="primary"
                    size="small"
                    onClick={() => copy(this.copyScanData())}
                  >
                    Copy selected
                  </Button>
                )}
              </CopyToClipboard>
              <Button
                className="buttonStyles"
                variant="contained"
                color="primary"
                size="small"
                onClick={() => this.downloadScanData()}
              >
                Download
              </Button>
              <Button
                className="buttonStyles"
                variant="contained"
                color="primary"
                size="small"
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
          {/* Select Alert */}
          <SelectAlert
            status={this.state.selectAlert}
            onClose={this.onToggleSelectAlert}
          />
        </FullColumn>
      </LayoutWrapper>
    );
  }
}
const mapSateToProps = state => {
  return {
    datas: state.ScanData.listData,
    profile: state.Auth.profile
  };
};

const mapDispatchToProps = {
  getScanData: scanDataActions.getListData,
  deleteScanData: scanDataActions.delete,
  deleteMultiScanData: scanDataActions.deleteMulti,
  editScanData: scanDataActions.edit,
  addScanData: scanDataActions.add,
  downloadScanData: scanDataActions.download,
  search: scanDataActions.search
};
export default connect(
  mapSateToProps,
  mapDispatchToProps
)(ScanData);
