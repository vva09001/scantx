import React from "react";
import { connect } from "react-redux";
import {
  TextField,
  Select,
  MenuItem,
  InputLabel,
  FormControl
} from "@material-ui/core";
import Dialog, {
  DialogActions,
  DialogContent,
  DialogTitle
} from "components/uielements/dialogs";
import Button from "components/uielements/button";
import { CircularProgress } from "components/uielements/progress";

class Form extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      loading: false,
      params: {}
    };
  }
  componentDidMount() {}
  componentWillReceiveProps(nextProps) {
    this.setState({ params: { uid: nextProps.uid } });
  }
  onSuccess = () => {
    this.setState({
      loading: false
    });
  };
  onClose = () => {
    this.props.onToggle(false);
  };
  onSubmit = () => {
    this.setState(
      {
        loading: true
      },
      () => {
        this.props.onSubmit(this.state.params, this.onSuccess, this.onSuccess);
      }
    );
  };
  onChange = e => {
    const key = e.target.name;
    const value = e.target.value;
    this.setState({
      params: {
        ...this.state.params,
        [key]: value
      }
    });
  };
  render() {
    const { status } = this.props;
    return (
      <div>
        {this.state.loading ? (
          <CircularProgress />
        ) : (
          <Dialog open={status} onClose={this.onClose}>
            <DialogTitle>{"Add new scan data"}</DialogTitle>
            <DialogContent>
              <div>
                <TextField
                  required
                  name="stationName"
                  label="StationName"
                  margin="normal"
                  fullWidth
                  value={this.state.params.StationName}
                  onChange={e => this.onChange(e)}
                />
              </div>
              <div>
                <TextField
                  required
                  name="payload"
                  label="Payload"
                  margin="normal"
                  fullWidth
                  value={this.state.params.payload}
                  onChange={e => this.onChange(e)}
                />
              </div>
              <div>
                <TextField
                  required
                  name="dataType"
                  label="Data Type"
                  margin="normal"
                  fullWidth
                  value={this.state.params.dataType}
                  onChange={e => this.onChange(e)}
                />
              </div>
              <div>
                <TextField
                  required
                  name="fileName"
                  label="File Name"
                  margin="normal"
                  fullWidth
                  value={this.state.params.fileName}
                  onChange={e => this.onChange(e)}
                />
              </div>
              <FormControl style={{ width: "100%" }}>
                <InputLabel htmlFor="status">Status</InputLabel>
                <Select
                  // value="asdas"
                  value={this.state.params.status | 0}
                  inputProps={{
                    name: "status",
                    id: "status"
                  }}
                  onChange={e => this.onChange(e)}
                >
                  <MenuItem value={0}>Received</MenuItem>
                  <MenuItem value={1}>Processed</MenuItem>
                  <MenuItem value={2}>Failed</MenuItem>
                </Select>
              </FormControl>
            </DialogContent>
            {this.state.loading ? (
              <CircularProgress />
            ) : (
              <DialogActions>
                <Button onClick={this.onClose} color="primary">
                  Cancel
                </Button>
                <Button onClick={this.onSubmit} color="primary" autoFocus>
                  Submit
                </Button>
              </DialogActions>
            )}
          </Dialog>
        )}
      </div>
    );
  }
}

const mapStateToProps = state => {
  return {
    uid: state.Auth.profile.id
  };
};

const mapDispatchToProps = {};
export default connect(
  mapStateToProps,
  mapDispatchToProps
)(Form);
