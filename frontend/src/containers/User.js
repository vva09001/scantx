import React, { Component } from "react";
import LayoutWrapper from "../components/utility/layoutWrapper";
import Papersheet from "../components/utility/papersheet";
import { FullColumn } from "../components/utility/rowColumn";
import Button from "../components/uielements/button/index.js";
import Grid from "@material-ui/core/Grid";
import Table from "@material-ui/core/Table";
import TableBody from "@material-ui/core/TableBody";
import TableCell from "@material-ui/core/TableCell";
import TableRow from "@material-ui/core/TableRow";
import "../styles/style.css";

export default class User extends Component {
  constructor(props) {
    super(props);
  }
  handleAddUser = () => {
    this.props.history.push('/dashboard/add-user');
  }
  render() {
    return (
      <LayoutWrapper>
        <FullColumn>
          <Papersheet title="Users of company CSBG">
            <Table>
              <TableBody>
                <TableRow>
                  <TableCell>Christian Gatmann</TableCell>
                  <TableCell>gatmann@csbg.de</TableCell>
                  <TableCell>admin</TableCell>
                </TableRow>
                <TableRow>
                  <TableCell>Mark Mustermann</TableCell>
                  <TableCell>mustermann@csbg.de</TableCell>
                  <TableCell>input</TableCell>
                </TableRow>
                <TableRow>
                  <TableCell>Maria Musterfrau</TableCell>
                  <TableCell>musterfrau@csbg.de</TableCell>
                  <TableCell>reader</TableCell>
                </TableRow>
              </TableBody>
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
                onClick={this.handleAddUser}
              >
                Add new user
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
              >
                Delete selected
              </Button>
            </Grid>
          </Papersheet>
        </FullColumn>
      </LayoutWrapper>
    );
  }
}
