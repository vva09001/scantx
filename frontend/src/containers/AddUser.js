import React, { Component } from "react";
import { connect } from 'react-redux';
import LayoutWrapper from "components/utility/layoutWrapper";
import Button from "components/uielements/button/index.js";
import { CircularProgress } from 'components/uielements/progress';
import { userActions } from 'redux/actions';
import "../styles/style.css";
import {
  FormControl, 
  FormLabel, 
  FormControlLabel, 
  InputLabel, 
  Select, 
  Checkbox,
  Grid,
  TextField
} from "@material-ui/core";

class AddUser extends Component {
  constructor(props) {
    super(props);
    this.state = {
      parasm: {},
      loading: false
    }
  }

  onClick = () => {
    this.setState({
      loading: true
    }, () => {
      this.props.addUser({}, this.onSuccess, this.onSuccess);
    })
  }

  onSuccess = () => {
    this.setState({
      loading: false
    })
  }

  render() {
    return (
      <LayoutWrapper>
        <Grid container justify="center">
          <Grid item xs={8}>
            <form>
              <h2>Create a new ScanTX user</h2>
              <div>
                <TextField
                  required
                  id="username-input"
                  label="Username"
                  margin="normal"
                  fullWidth
                />
              </div>
              <div>
                <TextField
                  id="given-name-input"
                  label="Given Name"
                  margin="normal"
                  fullWidth
                />
              </div>
              <div>
                <TextField
                  id="family-name-input"
                  label="Family Name"
                  margin="normal"
                  fullWidth
                />
              </div>
              <div>
                <FormControl required fullWidth margin="normal">
                  <InputLabel htmlFor="type-account-input">
                    Type of Account
                  </InputLabel>
                  <Select native>
                    <option value={""}></option>
                    <option value={"commercial"}>Commercial</option>
                    <option value={"test"}>Test</option>
                    <option value={"private"}>Private</option>
                  </Select>
                </FormControl>
              </div>
              <div>
                <FormControl required fullWidth margin="normal">
                  <InputLabel htmlFor="role-input">Role</InputLabel>
                  <Select native>
                    <option value={""}></option>
                    <option value={"user-reader"}>User Reader</option>
                    <option value={"admin"}>Admin</option>
                    <option value={"superadmin"}>Superadmin</option>
                  </Select>
                </FormControl>
              </div>
              <div>
                <TextField
                  required
                  id="password-input"
                  type="Password"
                  label="Password"
                  margin="normal"
                  fullWidth
                />
              </div>
              <div>
                <TextField
                  required
                  id="email-input"
                  label="Email"
                  margin="normal"
                  fullWidth
                />
              </div>
              <div>
                <FormControl margin="normal">
                  <FormLabel>Please contact me by Email</FormLabel>
                  <FormControlLabel
                    value="contact-by-email"
                    control={<Checkbox color="primary" />}
                  />
                </FormControl>
              </div>
              <div>
                <FormControl margin="normal">
                  <FormLabel>Encryption active</FormLabel>
                  <FormControlLabel
                    value="encryption-active"
                    control={<Checkbox color="primary" />}
                  />
                </FormControl>
              </div>
              <br />
              <p>* Mandatory fields</p>
              {
                this.state.loading ? (
                  <CircularProgress />
                ) : (
                  <div>
                    <Button
                      className="buttonStyles"
                      variant="contained"
                      color="primary"
                      onClick={this.handleAddUser}
                    >
                      Save
                    </Button>
                    <Button
                      className="buttonStyles"
                      variant="contained"
                      color="default"
                    >
                      Cancel
                    </Button>
                  </div>
                )
              }
              
            </form>
          </Grid>
        </Grid>
      </LayoutWrapper>
    );
  }
}

const mapSateToProps = state => {
  return {
  };
};

const mapDispatchToProps = {
  addUser: userActions.addUser
};
export default connect(
  mapSateToProps,
  mapDispatchToProps
)(AddUser);