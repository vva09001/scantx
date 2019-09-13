import React from "react";
import { connect } from "react-redux";
import { TextField } from "@material-ui/core";
import Dialog, {
  DialogActions,
  DialogContent,
  DialogTitle
} from "components/uielements/dialogs";
import Button from "components/uielements/button";
import { CircularProgress } from "components/uielements/progress";

class AssignForm extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      loading: false,
      params: {}
    };
  }
  componentDidMount() {}
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
            <DialogTitle>{"Assign user to company"}</DialogTitle>
            <DialogContent>
              <div>
                <TextField
                  required
                  name="userName"
                  label="Username"
                  margin="normal"
                  fullWidth
                  value={this.state.params.userName}
                  onChange={e => this.onChange(e)}
                />
              </div>
              <div>
                <TextField
                  required
                  name="email"
                  label="Email"
                  margin="normal"
                  fullWidth
                  value={this.state.params.email}
                  onChange={e => this.onChange(e)}
                />
              </div>
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
  return {};
};

const mapDispatchToProps = {};
export default connect(
  mapStateToProps,
  mapDispatchToProps
)(AssignForm);
